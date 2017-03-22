using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Saguar.MvvmUtils.Common;

namespace Saguar.MvvmUtils.Helpers
{
    public class HelperMethods<T> where T : class
    {

        #region Properties

        private SemaphoreSlim _mutex;

        #endregion

        #region Lazy and thread-safe singleton
        private static readonly Lazy<HelperMethods<T>> _current = new Lazy<HelperMethods<T>>(() => new HelperMethods<T>());
        public static HelperMethods<T> Current => _current.Value;

        private HelperMethods()
        {
            _mutex = new SemaphoreSlim(1);
        }
        #endregion

        #region Async

        public async void ExecuteAsync(Action action, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken == default(CancellationToken))
                cancellationToken = CancellationToken.None;
            await Task.Factory.StartNew(action, cancellationToken);
        }

        public async void ExecuteConcurrentAsync(Action action, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken == default(CancellationToken))
                cancellationToken = CancellationToken.None;
            try
            {
                //sync on mutex object
                await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);

                await Task.Factory.StartNew(action, cancellationToken);

            }
            finally
            {
                _mutex.Release();
            }
        }

        public async Task<T> ExecuteAsync(Func<T> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken == default(CancellationToken))
                cancellationToken = CancellationToken.None;
            return await Task<T>.Factory.StartNew(action, cancellationToken);
        }

        public async Task<T> ExecuteConcurrentAsync(Func<T> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken == default(CancellationToken))
                cancellationToken = CancellationToken.None;
            try
            {
                //sync on mutex object
                await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);

                return await Task<T>.Factory.StartNew(action, cancellationToken);
            }
            finally
            {
                _mutex.Release();
            }
        }


        #endregion

        #region Exception handling

        public OperationResult<T> ExecuteSafely(Func<T> action)
        {
            var result = new OperationResult<T>();
            try
            {
                result.Result = action();
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
            return result;
        }

        public async Task<OperationResult<T>> ExecuteSafelyAsync(Func<T> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new OperationResult<T>();
            try
            {
                result.Result = await ExecuteAsync(action, cancellationToken);
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
            return result;
        }

        public async Task<OperationResult<T>> ExecuteSafelyConcurrentAsync(Func<T> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new OperationResult<T>();
            try
            {
                //sync on mutex object
                await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);

                result.Result = await ExecuteAsync(action, cancellationToken);
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
            finally
            {
                _mutex.Release();
            }
            return result;
        }


        #endregion

    }
}

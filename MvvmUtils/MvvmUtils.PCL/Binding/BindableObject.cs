using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Saguar.MvvmUtils.Binding
{
    public class BindableObject<T> : INotifyPropertyChanged
    {

        #region PropertyChanged/ing implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<U>(string propertyName, ref U field, U value)
        {
            if (EqualityComparer<U>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        protected bool Set<U>(ref U field, U value, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<U>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        #endregion

        public BindableObject(T val)
        {
            Value = val;
        }

        private T _OldValue;
        public T OldValue
        {
            get { return _OldValue; }
            set { Set(ref _OldValue, value); }
        }

        //Current Value 
        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                OldValue = _value;
                Set(ref _value, value);
            }
        }

        public T FormattedValue { get { return Format(); } }

        protected virtual T Format()
        {
            return Value;
        }

    }
}

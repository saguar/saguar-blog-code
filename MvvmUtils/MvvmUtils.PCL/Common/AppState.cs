using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MvvmUtils.PCL.Common
{
    public class AppState : INotifyPropertyChanged
    {

        #region PropertyChanged/ing implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool Set<U>(string propertyName, ref U field, U value)
        {
            if (EqualityComparer<U>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        private bool Set<U>(ref U field, U value, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<U>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region Properties

        public enum StageLevel
        {
            Development, Staging, Production
        }

        //AppName property
        private string _appName;
        public string AppName
        {
            get { return _appName; }
            set { Set(ref _appName, value); }
        }

        //AppVersion property
        private string _appVersion;
        public string AppVersion
        {
            get { return _appVersion; }
            set { Set(ref _appVersion, value); }
        }

        //AppStageLevel property
        private StageLevel _appStage;
        public StageLevel AppStageLevel
        {
            get { return _appStage; }
            set { Set(ref _appStage, value); }
        }

        #endregion

        #region Lazy and thread-safe singleton
        private static readonly Lazy<AppState> _current = new Lazy<AppState>(() => new AppState());
        public static AppState Current => _current.Value;

        private AppState()
        {
            //Init class properties here, maybe loading from config file
        }
        #endregion

    }
}

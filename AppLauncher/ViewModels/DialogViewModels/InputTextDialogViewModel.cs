using SimpleMvvm;
using SimpleMvvm.Command;
using System;

namespace AppLauncher.ViewModels.DialogViewModels
{
    public class InputTextDialogViewModel : ViewModelBase
    {
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand OkCommand { get; set; }

        public Action ClearTextBoxAction { get; set; }
        public Action CloseWindowAction { get; set; }

        public bool Ok_Invoked { get; private set; }

        private void Cancel(object obj)
        {
            ClearTextBoxAction?.Invoke();
            CloseWindowAction?.Invoke();
        }

        private void Ok(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                System.Windows.MessageBox.Show("请输入文本。");
            }
            else
            {
                Ok_Invoked = true;
                CloseWindowAction?.Invoke();
            }
        }

        protected override void Init()
        {
            base.Init();

            CancelCommand = new DelegateCommand(Cancel);
            OkCommand = new DelegateCommand<string>(Ok);
        }
    }
}

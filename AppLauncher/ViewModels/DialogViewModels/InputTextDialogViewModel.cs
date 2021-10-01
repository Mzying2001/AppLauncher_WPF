using SimpleMvvm;
using SimpleMvvm.Command;
using System;
using System.Windows.Input;

namespace AppLauncher.ViewModels.DialogViewModels
{
    public class InputTextDialogViewModel : ViewModelBase
    {
        public ICommand CancelCommand { get; set; }
        public ICommand OkCommand { get; set; }

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

        public InputTextDialogViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            OkCommand = new DelegateCommand<string>(Ok);
        }
    }
}

using SimpleMvvm.Locator;

namespace AppLauncher.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        public ViewModelLocator()
        {
            Register<MainWindowViewModel>();
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get => GetInstance<MainWindowViewModel>();
        }
    }
}

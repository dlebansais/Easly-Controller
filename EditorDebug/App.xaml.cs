using System.Windows;

namespace EditorDebug
{
    public partial class App : Application
    {
        private void OnActivated(object sender, System.EventArgs e)
        {
            (MainWindow as MainWindow).OnActivated();
        }

        private void OnDeactivated(object sender, System.EventArgs e)
        {
            (MainWindow as MainWindow).OnDeactivated();
        }
    }
}

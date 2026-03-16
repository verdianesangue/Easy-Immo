namespace EasyImmoMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("PropertyPage", typeof(Views.PropertyPage));
        }
    }
}

namespace Pippin.UI.Screens
{
    public interface IScreenFactory
    {
        IScreen CreateScreen(object screenSubject);
    }
}
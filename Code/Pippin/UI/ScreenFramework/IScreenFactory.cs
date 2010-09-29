namespace Odin.UI.Infrastructure.ScreenFramework
{
    public interface IScreenFactory
    {
        IScreen CreateScreen(object screenSubject);
    }
}
public class PreloaderWindow : Window
{
    public bool IsLoaded { get; private set; }

    public override void Show(bool animation = true)
    {
        IsLoaded = false;

        base.Show(animation);
    }

    public override void OnEndShowAnimation()
    {
        base.OnEndShowAnimation();

        IsLoaded = true;
    }

    public override void OnEndHideAnimation()
    {
        base.OnEndHideAnimation();

        IsLoaded = false;
    }
}

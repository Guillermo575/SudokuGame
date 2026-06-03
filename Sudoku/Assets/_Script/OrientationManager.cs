using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    #region Variables
    public bool isLeftHanded = false;
    public enum OrientationMode
    {
        Vertical,
        HorizontalLeft,
        HorizontalRight
    }
    public OrientationMode CurrentOrientation { get; private set; }
    private bool _isMobilePlatform;
    #endregion

    #region Awake & Start
    void Awake()
    {
        _isMobilePlatform = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        CreateSingleton();
    }
    void Update()
    {
        CurrentOrientation = _isMobilePlatform ? CheckOrientationChangeMobile() : CheckOrientationChange();
    }
    #endregion

    #region Orientation
    private OrientationMode CheckOrientationChangeMobile()
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            return OrientationMode.Vertical;
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            return (isLeftHanded ? OrientationMode.HorizontalLeft : OrientationMode.HorizontalRight);
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            return (isLeftHanded ? OrientationMode.HorizontalRight : OrientationMode.HorizontalLeft);
        }
        return OrientationMode.Vertical;
    }
    private OrientationMode CheckOrientationChange()
    {
        if (Screen.width > Screen.height)
        {
            return (isLeftHanded ? OrientationMode.HorizontalLeft : OrientationMode.HorizontalRight);
        }
        else
        {
            return OrientationMode.Vertical;
        }
    }
    #endregion

    #region Singleton
    private static OrientationManager SingletonGameManager;
    private OrientationManager() { }
    private void CreateSingleton()
    {
        if (SingletonGameManager == null)
        {
            SingletonGameManager = this;
        }
        else
        {
            Debug.LogError("Ya existe una instancia de esta clase");
        }
    }
    public static OrientationManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion	
}
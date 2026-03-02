using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    public static AudioMgr Instance;

    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource conveyorSource;
    [SerializeField] private AudioSource sewageSource;

    [SerializeField] private AudioSource furnaceStartSource;
    [SerializeField] private AudioSource furnaceActiveSource;
    [SerializeField] private AudioSource furnaceEndSource;

    [SerializeField] private AudioSource washingSource;

    [SerializeField] private AudioSource binSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        //PlayAmbient();
        //PlayConveyor();
    }

    public void PlayAmbient()
    {
        if (!ambientSource.isPlaying)
            ambientSource.Play();
    }

    public void PlayConveyor()
    {
        if (!conveyorSource.isPlaying)
            conveyorSource.Play();
    }

    public void PlaySewage()
    {
        if(!sewageSource.isPlaying)
            sewageSource.Play();
    }

    public void PlayFurnaceStart()
    {
        furnaceStartSource.Play();
        furnaceActiveSource.loop = true;
        furnaceActiveSource.Play();
    }

    public void PlayFurnaceEnd()
    {
        furnaceActiveSource.Stop();
        furnaceEndSource.Play();
    }

    public void PlayWashing()
    {
        washingSource.loop = true;
        washingSource.Play();
    }

    public void StopWashing()
    {
        washingSource.Stop();
    }

    public void PlayBin()
    {
        binSource.Play();
    }

    public void StopAll()
    {
        ambientSource.Stop();
        conveyorSource.Stop();
        sewageSource.Stop();
    }
}

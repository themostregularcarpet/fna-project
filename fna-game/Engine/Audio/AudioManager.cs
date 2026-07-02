using FMOD;
using FMOD.Studio;

namespace Videogame.Engine.Audio;

public static class AudioManager
{
    private static FMOD.Studio.System system;
    private static Bank bank;
    private static Bank stringBank;

    public static void Initialize()
    {
        FMOD.Studio.System.create(out system);

        system.initialize(1024, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);
        
        string masterPath = Path.Combine(Core.Content.RootDirectory, "Audio", "Master.bank");
        string stringPath = Path.Combine(Core.Content.RootDirectory, "Audio", "Master.strings.bank");

        system.loadBankFile(masterPath, LOAD_BANK_FLAGS.NORMAL, out bank);
        system.loadBankFile(stringPath, LOAD_BANK_FLAGS.NORMAL, out stringBank);
    }

    public static EventInstance PlayEvent(string eventName)
    {
        system.getEvent(eventName, out EventDescription evDesc);
        evDesc.createInstance(out EventInstance evInst);
        evInst.start();
        return evInst;
    }

    public static void SetParameter(EventInstance evInst, string name, float value) => evInst.setParameterByName(name, value);

    public static void SetParameter(EventInstance evInst, PARAMETER_ID paramId, float value) => evInst.setParameterByID(paramId, value);

    public static void SetGlobalParameter(string name, float value) => system.setParameterByName(name, value);

    public static void SetGlobalParameter(PARAMETER_ID paramId, float value) => system.setParameterByID(paramId, value);

    public static PARAMETER_ID GetParameterId(string eventPath, string paramName)
    {
        system.getEvent(eventPath, out EventDescription evDesc);
        evDesc.getParameterDescriptionByName(paramName, out PARAMETER_DESCRIPTION paramDesc);
        return paramDesc.id;
    }

    public static EventDescription GetEventDescription(string eventPath)
    {
        system.getEvent(eventPath, out EventDescription evDesc);
        return evDesc;
    }

    public static void Update()
    {
        system.update();
    }

    public static void Dispose()
    {
        bank.unload();
        stringBank.unload();
        system.release();
    }
}
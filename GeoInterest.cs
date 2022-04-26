using Modding;
using UnityEngine;
using System;
using System.Collections;

namespace GeoInterestMod
{
    public class GeoInterest : Mod, ITogglableMod, ICustomMenuMod
    {
        public static GeoInterest instance;

        public override string GetVersion() => "1.0.0";

        public GameObject gameObject { get; private set; }

        public bool ToggleButtonInsideMenu => true;

        public override void Initialize()
        {
            if (instance != null) return;

            instance = this;

            gameObject = new GameObject();
            gameObject.AddComponent<InterestTask>();

            gameObject.SetActive(true);

            UnityEngine.Object.DontDestroyOnLoad(gameObject);

            InterestTimer.Initialize();
            InterestTimer.SetActive(true);

            On.HeroController.Awake += OnSaveFileLoad;
            On.QuitToMenu.Start += OnSaveFileClose;
            On.HeroController.Pause += OnPause;
            On.HeroController.UnPause += OnResume;
        }

        public void Unload()
        {
            instance = null;

            UnityEngine.Object.Destroy(gameObject);

            InterestTimer.Destroy();

            On.HeroController.Awake -= OnSaveFileLoad;
            On.QuitToMenu.Start -= OnSaveFileClose;
            On.HeroController.Pause -= OnPause;
            On.HeroController.UnPause -= OnResume;
        }

        private void OnSaveFileLoad(On.HeroController.orig_Awake orig, HeroController self)
        {
            orig(self);

            InterestTask.StartTaskWithDelay(TimeSpan.FromSeconds(5));
        }

        private IEnumerator OnSaveFileClose(On.QuitToMenu.orig_Start orig, QuitToMenu self)
        {
            InterestTask.StopTask();

            return orig(self);
        }

        private void OnPause(On.HeroController.orig_Pause orig, HeroController self)
        {
            InterestTask.PauseTask();

            orig(self);
        }

        private void OnResume(On.HeroController.orig_UnPause orig, HeroController self)
        {
            InterestTask.ResumeTask();

            orig(self);
        }

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            return MenuMod.GetMenuScreen(modListMenu, toggleDelegates.Value);
        }
    }
}

using Modding;
using Modding.Menu;
using Modding.Menu.Config;
using UnityEngine.UI;
using System;

namespace GeoInterestMod
{
    public class MenuMod
    {
        public static MenuScreen GetMenuScreen(MenuScreen returnScreen, ModToggleDelegates dels)
        {
            void cancelAction(MenuSelectable _) => UIManager.instance.GoToDynamicMenu(returnScreen);

            MenuBuilder menuBuilder = MenuUtils.CreateMenuBuilderWithBackButton("GeoInterestMod", returnScreen, out MenuButton _);

            HorizontalOptionConfig modToggleConfig = new HorizontalOptionConfig()
            {
                Label = "Toggle Mod",
                Options = new string[] { "On", "Off" },
                ApplySetting = (_, i) => dels.SetModEnabled(i == 0),
                RefreshSetting = (s, _) => s.optionList.SetOptionTo(dels.GetModEnabled() ? 0 : 1),
                CancelAction = cancelAction
            };

            HorizontalOptionConfig timerDisplayToggleConfig = new HorizontalOptionConfig()
            {
                Label = "Display Timer?",
                Options = new string[] { "Yes", "No" },
                ApplySetting = (_, i) =>
                {
                    InterestTask.ShouldDisplayTimer = i == 0;
                    InterestTimer.SetActive(InterestTask.ShouldDisplayTimer);
                },
                RefreshSetting = (s, _) => s.optionList.SetOptionTo(InterestTask.ShouldDisplayTimer ? 0 : 1),
                CancelAction = cancelAction
            };

            string[] interestRates = { "5%", "10%", "15%", "25%", "50%", "75%", "100%" };

            HorizontalOptionConfig interestRateConfig = new HorizontalOptionConfig()
            {
                Label = "Interest Rate",
                Options = interestRates,
                ApplySetting = (_, i) =>
                {
                    var value = Convert.ToDouble(interestRates[i].Replace("%", "")) / 100d + 1d;

                    InterestTask.Edit(value);
                },
                RefreshSetting = (s, _) =>
                {
                    var currentValue = (InterestTask.InterestRate - 1) * 100;
                    var displayValue = Array.IndexOf(interestRates, $"{currentValue}%");

                    s.optionList.SetOptionTo(displayValue);
                },
                CancelAction = cancelAction
            };

            string[] interestIntervals = { "60s", "90s", "120s", "5s", "10s", "15s", "20s", "25s", "30s", "35s", "40s", "45s", "50s" };

            HorizontalOptionConfig interestIntervalConfig = new HorizontalOptionConfig()
            {
                Label = "Interest Interval",
                Options = interestIntervals,
                ApplySetting = (_, i) =>
                {
                    var value = Convert.ToInt32(interestIntervals[i].Replace("s", ""));
                    var timeSpan = TimeSpan.FromSeconds(value);

                    InterestTask.Edit(timeSpan);
                },
                RefreshSetting= (s, _) =>
                {
                    var currentVal = $"{InterestTask.InterestInterval.TotalSeconds}s";
                    var displayValue = Array.IndexOf(interestIntervals, currentVal);

                    s.optionList.SetOptionTo(displayValue);
                },
                CancelAction = cancelAction
            };

            menuBuilder.AddContent(
                RegularGridLayout.CreateVerticalLayout(105f),
                c =>
                {
                    c.AddHorizontalOption("ToggleModOption", modToggleConfig);
                    c.AddHorizontalOption("ToggleInterestTimerDisplay", timerDisplayToggleConfig);
                    c.AddHorizontalOption("CycleInterestRate", interestRateConfig);
                    c.AddHorizontalOption("CycleInterestInterval", interestIntervalConfig);
                });

            return menuBuilder.Build();
        }
    }
}
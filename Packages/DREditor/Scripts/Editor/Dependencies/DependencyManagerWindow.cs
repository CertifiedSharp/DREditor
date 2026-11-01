//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dependencies
{
    /// <summary>
    ///
    /// </summary>
    public class DependencyManagerWindow : EditorWindow
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly string WelcomedKey = "DREWelcomed";


        /// <summary>
        ///
        /// </summary>
        protected List<DependencyInitializerBase> dependencies;

        /// <summary>
        ///
        /// </summary>
        protected GUIStyle largeLabelStyle;

        /// <summary>
        ///
        /// </summary>
        protected DependencyInitializerBase currentDependency;


        protected virtual void OnEnable()
        {
            dependencies = CreateInitializers().OrderBy(type => type.Priority).ToList();

            if (!DependencyManager.AllDependenciesInstalled())
                DisplayStartUpMessages();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static List<DependencyInitializerBase> CreateInitializers()
        {
            List<DependencyInitializerBase> dependencies = new();

            var types = GenericTypeList<DependencyInitializerBase>.GetList().Count != 0 ?
                GenericTypeList<DependencyInitializerBase>.GetList() : GenericTypeList<DependencyInitializerBase>.GetInheritedClasses();

            foreach (var type in types)
                dependencies.Add((DependencyInitializerBase)Activator.CreateInstance(type));

            return dependencies;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayStartUpMessages()
        {
            HandyFields.SystemMessage(WelcomedKey,
                "Welcome to DREditor!",
                "REPORT: " +
                "\nWelcome to DREditor, I am POD-DR3, the console assistant for this engine." +
                "\nBefore a death game can be created, there are some initial steps that must be taken." +
                "\nQUERY: Are you ready to start?",
                "I'm Ready");

            HandyFields.SystemMessage(DependencyAutoInstaller.AutoInstallKey,
                "Import Auto Installable Dependencies?",
                "REPORT:" +
                "\nThere are a couple unity packages that need to be installed " +
                "that this unit can take care of for you." +
                "\nQUERY: Auto install applicable dependencies?",
                "Confirm",
                DependencyAutoInstaller.Install);
        }


        protected virtual void OnGUI()
        {
            DisplayTitle();

            if (DependenciesInitialized())
                DisplayManager();
            else
                OnSystemInitializationReset();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTitle()
        {
            largeLabelStyle ??= new(EditorStyles.boldLabel)
            {
                fontSize = 24,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Welcome to the Dependency Manager", largeLabelStyle);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DependenciesInitialized() => DependencyAutoInstaller.Initialized();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnSystemInitializationReset() => 
            GUILayout.Label("Follow the systems instructions!", largeLabelStyle);

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayManager()
        {
            HandyFields.Label("REPORT: Before using DREditor, all items listed must have check marks and no issues displayed.");

            DisplayIssues();

            DisplayDependencies();

            DisplayButtons();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayIssues()
        {
            if (!DependencyManager.AllDependenciesInstalled())
                EditorGUILayout.HelpBox("ALERT: Not all dependencies have been resolved!", MessageType.Error, true);

            if (!DependencyAutoInstaller.Initialized())
                EditorGUILayout.HelpBox("ALERT: Automatic dependencies have not been installed!", MessageType.Error, true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDependencies()
        {
            dependencies ??= CreateInitializers().OrderBy(type => type.Priority).ToList();

            foreach (var dependency in dependencies)
                DisplayDependency(dependency);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentDependency"></param>
        protected virtual void DisplayDependency(DependencyInitializerBase currentDependency)
        {
            SetCurrentDependency(currentDependency);

            GUILayout.BeginHorizontal(EditorStyles.boldLabel);

            DisplayDependencyData();

            DisplayInstallStateIcon();

            GUILayout.EndHorizontal();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentDependency"></param>
        protected virtual void SetCurrentDependency(DependencyInitializerBase currentDependency) =>
            this.currentDependency = currentDependency;

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDependencyData() =>
            HandyFields.Label(currentDependency.DisplayName, EditorStyles.boldLabel);

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayInstallStateIcon()
        {
            if (CurrentDependencyCompleted())
                HandyFields.DisplayIcon("Checkmark", Color.green);
            else
                HandyFields.DisplayIcon("console.warnicon", Color.yellow);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CurrentDependencyCompleted() =>
            currentDependency.Completed || (!currentDependency.Required && currentDependency.Ignored);

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayButtons() => CustomLayoutUtility.DisplayHorizontal(NextButton, ResetButton);

        /// <summary>
        ///
        /// </summary>
        protected virtual void NextButton() =>
            HandyFields.Button("Install Next Dependency/Refresh", () => DependencyManager.InstallDependencies(dependencies));

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetButton() =>
            HandyFields.Button("Reset System", DependencyManager.ResetDependencies);
    }
}
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using vs_commitizen.Settings;
using vs_commitizen.vs.Settings;

namespace ViewTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            IoC.Container.Inject<IUserSettings>(new DummyUserSettings());
            IoC.Container.Inject<IConfigFileProvider>(new DummyConfigFileProvider());
            IoC.Container.Inject<IServiceProvider>(new DummyServiceProvider());

            InitializeComponent();
        }

        public class DummyUserSettings : IUserSettings
        {
            public int MaxLineLength { get; set; } = 80;
        }

        internal class DummyServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType)
            {
                return 1;
            }
        }

        internal class DummyConfigFileProvider : IConfigFileProvider
        {
            public Task<IList<T>> GetCommitTypesAsync<T>() where T : class
            {
                throw new System.NotImplementedException();
            }

            public Task<string> GetConfigUserProfilePathAsync()
            {
                throw new NotImplementedException();
            }

            public Task<string> TryGetLocalConfigAsync()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}

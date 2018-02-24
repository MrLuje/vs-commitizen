﻿/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Limited Public License (MS-LPL).
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;

namespace vs_commitizen.vs
{
    /// <summary>
    /// Team Explorer plugin common base class.
    /// </summary>
    public class TeamExplorerBase : IDisposable, INotifyPropertyChanged
    {
        #region Members

        private bool m_contextSubscribed = false;

        #endregion

        /// <summary>
        /// Get/set the service provider.
        /// </summary>
        public IServiceProvider ServiceProvider
        {
            get { return m_serviceProvider; }
            set
            {
                // Unsubscribe from Team Foundation context changes
                if (m_serviceProvider != null)
                {
                    UnsubscribeContextChanges();
                }

                m_serviceProvider = value;

                // Subscribe to Team Foundation context changes
                if (m_serviceProvider != null)
                {
                    SubscribeContextChanges();
                }
            }
        }
        private IServiceProvider m_serviceProvider = null;

        /// <summary>
        /// Get the requested service from the service provider.
        /// </summary>
        public T GetService<T>()
        {
            Debug.Assert(this.ServiceProvider != null, "GetService<T> called before service provider is set");
            if (this.ServiceProvider != null)
            {
                return (T)this.ServiceProvider.GetService(typeof(T));
            }

            return default(T);
        }

        private Dictionary<string, Guid> _notifications = new Dictionary<string, Guid>();

        /// <summary>
        /// Show a notification in the Team Explorer window.
        /// </summary>
        protected Guid ShowNotification(string message, NotificationType type)
        {
            var teamExplorer = GetService<ITeamExplorer>();
            if (teamExplorer != null)
            {
                var guid = Guid.NewGuid();

                if (_notifications.ContainsKey(message) && teamExplorer.IsNotificationVisible(_notifications[message]))
                {
                    teamExplorer.HideNotification(_notifications[message]);
                    _notifications.Remove(message);
                }

                if (!_notifications.ContainsKey(message))
                    _notifications.Add(message, guid);

                teamExplorer.ShowNotification(message, type, NotificationFlags.None, null, guid);
                return guid;
            }

            return Guid.Empty;
        }

        #region IDisposable

        /// <summary>
        /// Dispose.
        /// </summary>
        public virtual void Dispose()
        {
            UnsubscribeContextChanges();
        }

        #endregion

        #region INotifyPropertyChanged

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Team Foundation Context

        /// <summary>
        /// Subscribe to context changes.
        /// </summary>
        protected void SubscribeContextChanges()
        {
            Debug.Assert(this.ServiceProvider != null, "ServiceProvider must be set before subscribing to context changes");
            if (this.ServiceProvider == null || m_contextSubscribed)
            {
                return;
            }

            var tfContextManager = GetService<ITeamFoundationContextManager>();
            if (tfContextManager != null)
            {
                tfContextManager.ContextChanged += ContextChanged;
                m_contextSubscribed = true;
            }
        }

        /// <summary>
        /// Unsubscribe from context changes.
        /// </summary>
        protected void UnsubscribeContextChanges()
        {
            if (this.ServiceProvider == null || !m_contextSubscribed)
            {
                return;
            }

            var tfContextManager = GetService<ITeamFoundationContextManager>();
            if (tfContextManager != null)
            {
                tfContextManager.ContextChanged -= ContextChanged;
            }
        }

        /// <summary>
        /// ContextChanged event handler.
        /// </summary>
        protected virtual void ContextChanged(object sender, ContextChangedEventArgs e)
        {
        }

        /// <summary>
        /// Get the current Team Foundation context.
        /// </summary>
        protected ITeamFoundationContext CurrentContext
        {
            get
            {
                var tfContextManager = GetService<ITeamFoundationContextManager>();
                if (tfContextManager != null)
                {
                    return tfContextManager.CurrentContext;
                }

                return null;
            }
        }

        #endregion
    }
}

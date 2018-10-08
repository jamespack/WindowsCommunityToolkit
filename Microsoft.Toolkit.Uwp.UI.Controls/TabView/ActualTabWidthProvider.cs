﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <inheritdoc/>
    [Bindable]
    public class ActualTabWidthProvider : ITabWidthProvider
    {
        /// <inheritdoc/>
        public bool IsSelectedTabWidthDifferent => true;

        /// <inheritdoc/>
        public int MinimumWidth => 0;

        /// <summary>
        /// Gets or sets the amount of space to leave for the close button of the selected tab.
        /// </summary>
        public int TabCloseMargin { get; set; } = 36;

        /// <inheritdoc/>
        public IEnumerable<double> ProvideWidth(IEnumerable<TabViewItem> tabs, object items, double availableWidth, TabView parent)
        {
            double minwidth = 0;
            if (parent.Resources != null && parent.Resources.TryGetValue("ListViewItemMinWidth", out object value) && value is double dbl)
            {
                minwidth = dbl;
            }

            foreach (var tab in tabs)
            {
                var size = GetInitialActualWidth(tab);

                if (size <= double.Epsilon && tab.ActualWidth > minwidth)
                {
                    SetInitialActualWidth(tab, tab.ActualWidth);
                    size = tab.ActualWidth;
                }

                // If it's selected leave extra room for close button and add right-margin.
                yield return (tab.IsSelected && tab.IsClosable ? TabCloseMargin : 0) + size;
            }
        }

        private static double GetInitialActualWidth(TabViewItem obj)
        {
            return (double)obj.GetValue(InitialActualWidthProperty);
        }

        private static void SetInitialActualWidth(TabViewItem obj, double value)
        {
            obj.SetValue(InitialActualWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty InitialActualWidthProperty =
            DependencyProperty.RegisterAttached("InitialActualWidth", typeof(double), typeof(ActualTabWidthProvider), new PropertyMetadata(0.0));
    }
}

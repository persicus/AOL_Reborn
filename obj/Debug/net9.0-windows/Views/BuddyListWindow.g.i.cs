﻿#pragma checksum "..\..\..\..\Views\BuddyListWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "CC09E52CD306C6104D46062DEE240C7B097B5442"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace AOL_Reborn.Views {
    
    
    /// <summary>
    /// BuddyListWindow
    /// </summary>
    public partial class BuddyListWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 60 "..\..\..\..\Views\BuddyListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl MainTabControl;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\..\Views\BuddyListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView BuddyTreeView;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\Views\BuddyListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddBuddyButton;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\..\Views\BuddyListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveBuddyButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AOL_Reborn;component/views/buddylistwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\BuddyListWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 36 "..\..\..\..\Views\BuddyListWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.SignOff_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MainTabControl = ((System.Windows.Controls.TabControl)(target));
            return;
            case 3:
            this.BuddyTreeView = ((System.Windows.Controls.TreeView)(target));
            
            #line 66 "..\..\..\..\Views\BuddyListWindow.xaml"
            this.BuddyTreeView.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.BuddyTreeView_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AddBuddyButton = ((System.Windows.Controls.Button)(target));
            
            #line 98 "..\..\..\..\Views\BuddyListWindow.xaml"
            this.AddBuddyButton.Click += new System.Windows.RoutedEventHandler(this.AddBuddyButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.RemoveBuddyButton = ((System.Windows.Controls.Button)(target));
            
            #line 112 "..\..\..\..\Views\BuddyListWindow.xaml"
            this.RemoveBuddyButton.Click += new System.Windows.RoutedEventHandler(this.RemoveBuddyButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


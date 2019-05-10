﻿#pragma checksum "..\..\..\UserControllers\ListsUC.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8B47D8D71E146A25F379A424D89916584BD280BB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MrJobProject.Converters;
using MrJobProject.UserControllers;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace MrJobProject.UserControllers {
    
    
    /// <summary>
    /// ListsUC
    /// </summary>
    public partial class ListsUC : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\..\UserControllers\ListsUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ListOfMonths;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\UserControllers\ListsUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ListOfYears;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\UserControllers\ListsUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox searchTextBox;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\UserControllers\ListsUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView workersListView;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\UserControllers\ListsUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button allButton;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\UserControllers\ListsUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button noneButton;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\UserControllers\ListsUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button pdfButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MrJobProject;component/usercontrollers/listsuc.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControllers\ListsUC.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ListOfMonths = ((System.Windows.Controls.ComboBox)(target));
            
            #line 33 "..\..\..\UserControllers\ListsUC.xaml"
            this.ListOfMonths.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListOfMonths_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ListOfYears = ((System.Windows.Controls.ComboBox)(target));
            
            #line 47 "..\..\..\UserControllers\ListsUC.xaml"
            this.ListOfYears.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListOfYears_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.searchTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 59 "..\..\..\UserControllers\ListsUC.xaml"
            this.searchTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SearchTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.workersListView = ((System.Windows.Controls.ListView)(target));
            
            #line 67 "..\..\..\UserControllers\ListsUC.xaml"
            this.workersListView.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.WorkersListView_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.allButton = ((System.Windows.Controls.Button)(target));
            
            #line 85 "..\..\..\UserControllers\ListsUC.xaml"
            this.allButton.Click += new System.Windows.RoutedEventHandler(this.AllButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.noneButton = ((System.Windows.Controls.Button)(target));
            
            #line 96 "..\..\..\UserControllers\ListsUC.xaml"
            this.noneButton.Click += new System.Windows.RoutedEventHandler(this.NoneButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.pdfButton = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


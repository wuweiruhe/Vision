﻿#pragma checksum "..\..\..\..\Views\Pages\ParamsConfigControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "427EEBF598182DF2183524411DAC4B7431C8C6B9A686AA05BDD792E784751E08"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using VMControls.WPF.Release;
using Vision_NBB.Views.Pages;


namespace Vision_NBB.Views.Pages {
    
    
    /// <summary>
    /// ParamsConfigControl
    /// </summary>
    public partial class ParamsConfigControl : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\..\Views\Pages\ParamsConfigControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Vision_NBB.Views.Pages.ParamsConfigControl mainWin;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Views\Pages\ParamsConfigControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VMControls.WPF.Release.VmParamsConfigWithRenderControl config;
        
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
            System.Uri resourceLocater = new System.Uri("/Vision_NBB;component/views/pages/paramsconfigcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Pages\ParamsConfigControl.xaml"
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
            this.mainWin = ((Vision_NBB.Views.Pages.ParamsConfigControl)(target));
            
            #line 9 "..\..\..\..\Views\Pages\ParamsConfigControl.xaml"
            this.mainWin.Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\..\Views\Pages\ParamsConfigControl.xaml"
            this.mainWin.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.config = ((VMControls.WPF.Release.VmParamsConfigWithRenderControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


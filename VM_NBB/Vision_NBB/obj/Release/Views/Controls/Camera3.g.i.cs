﻿#pragma checksum "..\..\..\..\Views\Controls\Camera3.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "077DE88686525056D9D66D274BD687CC9C6FCED8029956D765E04C2F848147C1"
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
using Vision_NBB;
using Vision_NBB.Model;
using Vision_NBB.Views.Controls;


namespace Vision_NBB.Views.Controls {
    
    
    /// <summary>
    /// Camera3
    /// </summary>
    public partial class Camera3 : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 50 "..\..\..\..\Views\Controls\Camera3.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Vision_NBB.ButtonEx buttonExecuteOnce;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\Views\Controls\Camera3.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Vision_NBB.ButtonEx buttonContinuExecute;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\..\Views\Controls\Camera3.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Vision_NBB.ButtonEx buttonStopExecute;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\Views\Controls\Camera3.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VMControls.WPF.Release.VmRenderControl vm;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\Views\Controls\Camera3.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView tvProperties;
        
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
            System.Uri resourceLocater = new System.Uri("/Vision_NBB;component/views/controls/camera3.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Controls\Camera3.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 12 "..\..\..\..\Views\Controls\Camera3.xaml"
            ((Vision_NBB.Views.Controls.Camera3)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.buttonExecuteOnce = ((Vision_NBB.ButtonEx)(target));
            return;
            case 3:
            this.buttonContinuExecute = ((Vision_NBB.ButtonEx)(target));
            return;
            case 4:
            this.buttonStopExecute = ((Vision_NBB.ButtonEx)(target));
            return;
            case 5:
            this.vm = ((VMControls.WPF.Release.VmRenderControl)(target));
            return;
            case 6:
            this.tvProperties = ((System.Windows.Controls.TreeView)(target));
            
            #line 88 "..\..\..\..\Views\Controls\Camera3.xaml"
            this.tvProperties.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.TvProperties_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


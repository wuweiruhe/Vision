﻿#pragma checksum "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "03FEB893017DDD92C2A366AC15194C60B9C932A5F23C868543BC5EA1F97E04A7"
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
using Vision_NBB.Views.CalibPages;


namespace Vision_NBB.Views.CalibPages {
    
    
    /// <summary>
    /// DownGlueDistortionCalib
    /// </summary>
    public partial class DownGlueDistortionCalib : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 35 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VMControls.WPF.Release.VmRenderControl vm;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_SetROI;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ExecuteOnce;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_SaveCalibFile;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_ImageSource;
        
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
            System.Uri resourceLocater = new System.Uri("/Vision_NBB;component/views/calibpages/downgluedistortioncalib.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
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
            
            #line 9 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
            ((Vision_NBB.Views.CalibPages.DownGlueDistortionCalib)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 29 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Btn_close_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.vm = ((VMControls.WPF.Release.VmRenderControl)(target));
            return;
            case 4:
            this.btn_SetROI = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
            this.btn_SetROI.Click += new System.Windows.RoutedEventHandler(this.btn_SetROI_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_ExecuteOnce = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
            this.btn_ExecuteOnce.Click += new System.Windows.RoutedEventHandler(this.btn_ExecuteOnce_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Btn_SaveCalibFile = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
            this.Btn_SaveCalibFile.Click += new System.Windows.RoutedEventHandler(this.Btn_SaveCalibFile_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Btn_ImageSource = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\..\..\..\Views\CalibPages\DownGlueDistortionCalib.xaml"
            this.Btn_ImageSource.Click += new System.Windows.RoutedEventHandler(this.Btn_ImageSource_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


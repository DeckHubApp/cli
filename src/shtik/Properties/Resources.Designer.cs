﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace shtik.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("shtik.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] fira_sans_v7_latin_700_woff {
            get {
                object obj = ResourceManager.GetObject("fira_sans_v7_latin_700_woff", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] fira_sans_v7_latin_700_woff2 {
            get {
                object obj = ResourceManager.GetObject("fira_sans_v7_latin_700_woff2", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] fira_sans_v7_latin_700italic_woff {
            get {
                object obj = ResourceManager.GetObject("fira_sans_v7_latin_700italic_woff", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] fira_sans_v7_latin_700italic_woff2 {
            get {
                object obj = ResourceManager.GetObject("fira_sans_v7_latin_700italic_woff2", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] lato_v13_latin_italic_woff {
            get {
                object obj = ResourceManager.GetObject("lato_v13_latin_italic_woff", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] lato_v13_latin_italic_woff2 {
            get {
                object obj = ResourceManager.GetObject("lato_v13_latin_italic_woff2", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] lato_v13_latin_regular_woff {
            get {
                object obj = ResourceManager.GetObject("lato_v13_latin_regular_woff", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] lato_v13_latin_regular_woff2 {
            get {
                object obj = ResourceManager.GetObject("lato_v13_latin_regular_woff2", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (() =&gt; {
        ///    document.addEventListener(&quot;DOMContentLoaded&quot;, () =&gt; {
        ///        const previousLink = document.querySelector(&quot;#previous-link&quot;);
        ///        const nextLink = document.querySelector(&quot;#next-link&quot;);
        ///        document.addEventListener(&quot;keydown&quot;, (e) =&gt; {
        ///            const k = string.fromCharCode(e.keyCode);
        ///            if (k === &quot;.&quot; || k === &quot; &quot;) {
        ///                document.location.assign(nextLink.href);
        ///            }
        ///            if (k === &quot;,&quot;) {
        ///                document.location.assign(previousLi [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string script_js {
            get {
                return ResourceManager.GetString("script_js", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE html&gt;
        ///&lt;html&gt;
        ///&lt;head&gt;
        ///    &lt;meta charset=&quot;utf-8&quot; /&gt;
        ///    &lt;title&gt;{{title}}&lt;/title&gt;
        ///    &lt;!-- ReSharper disable once Html.PathError --&gt;
        ///    &lt;link href=&quot;theme.css&quot; rel=&quot;stylesheet&quot; /&gt;
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///&lt;div class=&quot;container container-{{layout}}&quot;&gt;
        ///    {{content}}
        ///&lt;/div&gt;
        ///&lt;nav&gt;
        ///    &lt;a id=&quot;previous-link&quot; href=&quot;{{previousIndex}}&quot;&gt;&amp;lt;&lt;/a&gt;
        ///    &lt;a id=&quot;next-link&quot; href=&quot;{{nextIndex}}&quot;&gt;&amp;gt;&lt;/a&gt;
        ///&lt;/nav&gt;
        ///&lt;!-- ReSharper disable once Html.PathError --&gt;
        ///&lt;script src=&quot;script.js&quot;&gt;&lt;/script&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string template_html {
            get {
                return ResourceManager.GetString("template_html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /*! normalize.css v7.0.0 | MIT License | github.com/necolas/normalize.css */button,hr,input{overflow:visible}audio,canvas,progress,video{display:inline-block}progress,sub,sup{vertical-align:baseline}[type=checkbox],[type=radio],legend{box-sizing:border-box;padding:0}html{line-height:1.15;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}body{margin:0}article,aside,details,figcaption,figure,footer,header,main,menu,nav,section{display:block}h1{font-size:2em;margin:.67em 0}figure{margin:1em 40px}hr{box-s [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string theme_css {
            get {
                return ResourceManager.GetString("theme_css", resourceCulture);
            }
        }
    }
}
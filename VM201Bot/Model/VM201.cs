using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VM201Bot.Model
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "xxx")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "xxx", IsNullable = false)]
    public partial class xml
    {

        private xmlStatus statusField;

        /// <remarks/>
        public xmlStatus status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "xxx")]
    public partial class xmlStatus
    {

        private xmlStatusLed[] ledsField;

        private xmlStatusTimer[] timersField;

        private xmlStatusInput inputField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("led", IsNullable = false)]
        public xmlStatusLed[] leds
        {
            get
            {
                return this.ledsField;
            }
            set
            {
                this.ledsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("timer", IsNullable = false)]
        public xmlStatusTimer[] timers
        {
            get
            {
                return this.timersField;
            }
            set
            {
                this.timersField = value;
            }
        }

        /// <remarks/>
        public xmlStatusInput input
        {
            get
            {
                return this.inputField;
            }
            set
            {
                this.inputField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "xxx")]
    public partial class xmlStatusLed
    {

        private byte idField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "xxx")]
    public partial class xmlStatusTimer
    {

        private byte idField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "xxx")]
    public partial class xmlStatusInput
    {

        private byte idField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}

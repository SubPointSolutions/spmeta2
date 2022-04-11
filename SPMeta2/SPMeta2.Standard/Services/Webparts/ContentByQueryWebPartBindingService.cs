using System.Collections.Generic;
using System.Linq;
using SPMeta2.Definitions;
using SPMeta2.Standard.Services.Webparts.ContentByQueryWebPart;
using System;
using SPMeta2.Exceptions;

namespace SPMeta2.Standard.Services.Webparts
{
    /// <summary>
    /// Enables fluent API to setup 'Content by Query' web part.
    /// </summary>
    public class ContentByQueryWebPartBindingService
    {
        #region constructors

        public ContentByQueryWebPartBindingService()
        {
            // Filters = new FilterValues();
        }

        #endregion

        #region properties

        protected Dictionary<string, List<DataMappingValue>> dataMapping = new Dictionary<string, List<DataMappingValue>>();
        protected List<DataMappingViewFieldValue> dataMappingView = new List<DataMappingViewFieldValue>();

        //public List<FilterPair> Pairs { get; set; }

        #endregion


        #region data mapings

        public virtual ContentByQueryWebPartBindingService AddDataMapping(string slotName, FieldDefinition field)
        {
            AddDataMapping(slotName, new[] { field });

            return this;
        }

        public virtual ContentByQueryWebPartBindingService AddDataMapping(string slotName, IEnumerable<FieldDefinition> fields)
        {
            foreach (var field in fields)
            {
                AddDataMapping(slotName, field.Id, field.InternalName, field.FieldType);
            }

            return this;
        }

        public virtual ContentByQueryWebPartBindingService AddDataMapping(string slotName,
            Guid fieldId, string fieldInternalName, string fieldType)
        {
            // slot processing
            if (!dataMapping.ContainsKey(slotName))
                dataMapping.Add(slotName, new List<DataMappingValue>());

            var existingSlot = dataMapping[slotName];

            if (existingSlot.FirstOrDefault(m => m.Id == fieldId) == null)
            {
                existingSlot.Add(new DataMappingValue
                {
                    SlotName = slotName,
                    Id = fieldId,
                    InternalName = fieldInternalName,
                    FieldType = fieldType
                });
            }


            AddDataMappingViewField(fieldId, fieldType);

            return this;
        }

        public virtual string DataMapping
        {
            get { return ComposeDataMapping(); }
        }

        protected virtual string ComposeDataMapping()
        {
            var resultValues = new List<string>();

            foreach (var slotName in dataMapping.Keys)
            {
                var topValue = dataMapping[slotName];
                var currentValues = new List<string>();

                foreach (var value in topValue)
                {
                    if (value == topValue.First())
                    {
                        currentValues.Add(string.Format("{0}{1}{2}{3}",
                            new string[]
                            {
                                value.SlotName,
                                value.Id.HasValue ? ":" + value.Id.Value.ToString("B") : string.Empty,
                                !string.IsNullOrEmpty(value.InternalName) ? "," + value.InternalName : string.Empty,
                                !string.IsNullOrEmpty(value.FieldType) ? "," + value.FieldType : string.Empty
                            }));
                    }
                    else
                    {
                        currentValues.Add(string.Format("{0}{1}{2}",
                           new string[]
                            {
                                value.Id.HasValue ?  value.Id.Value.ToString("B") : string.Empty,
                                !string.IsNullOrEmpty(value.InternalName) ? "," + value.InternalName : string.Empty,
                                !string.IsNullOrEmpty(value.FieldType) ? "," + value.FieldType : string.Empty
                            }));
                    }

                }

                resultValues.Add(string.Join(";", currentValues.ToArray()));
            }

            return string.Join(";|", resultValues.ToArray()) + ";|";
        }

        #endregion

        #region data mapping view fields

        public virtual ContentByQueryWebPartBindingService AddDataMappingViewField(FieldDefinition field)
        {
            return AddDataMappingViewFields(new[] { field });
        }

        public virtual ContentByQueryWebPartBindingService AddDataMappingViewFields(IEnumerable<FieldDefinition> fields)
        {
            foreach (var field in fields)
            {
                AddDataMappingViewField(field.Id, field.FieldType);
            }

            return this;
        }

        public virtual ContentByQueryWebPartBindingService AddDataMappingViewField(
            Guid fieldId, string fieldType)
        {
            // data mapping view processing
            if (dataMappingView.FirstOrDefault(m => m.Id == fieldId) == null)
            {
                dataMappingView.Add(new DataMappingViewFieldValue
                {
                    Id = fieldId,
                    FieldType = fieldType
                });
            }

            return this;
        }

        public virtual string DataMappingViewFields
        {
            get { return ComposeDataMappingViewFields(); }
        }

        protected virtual string ComposeDataMappingViewFields()
        {
            var resultValues = new List<string>();

            foreach (var value in dataMappingView)
            {
                resultValues.Add(string.Format("{0},{1}",
                    value.Id.HasValue ? value.Id.Value.ToString("B") : string.Empty,
                    //value.InternalName ?? string.Empty,
                    value.FieldType ?? string.Empty));
            }

            return string.Join(";", resultValues.ToArray());
        }

        #endregion

        #region filters

        public virtual ContentByQueryWebPartBindingService ClearFilter()
        {
            throw new SPMeta2NotImplementedException("method not implemented");

            //Filters.FilterValue1 = null;
            //Filters.FilterValue2 = null;
            //Filters.FilterValue3 = null;
        }

        public virtual ContentByQueryWebPartBindingService Filter(Guid? fieldIs, string value, string displayName, string type, string filtertOperator)
        {
            throw new SPMeta2NotImplementedException("method not implemented");
        }

        #endregion

    }

    //public class FilterPair
    //{
    //    public FilterValue FilterValue1 { get; set; }
    //    public FilterValueChainingOperator FilterValue1ChainOperator { get; set; }
    //}
}

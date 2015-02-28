using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Attributes.Regression
{
    /// <summary>
    /// Used by regression testing infrastructure to indicate properties which have to be changes with a new provision.
    /// </summary>
    public class ExpectUpdate : Attribute
    {

    }

    public class ExpectUpdateAsLCID : ExpectUpdate
    {

    }

    public class ExpectUpdateAsCamlQuery : ExpectUpdate
    {

    }
    public class ExpectUpdateAsInternalFieldName : ExpectUpdate
    {

    }
    public class ExpectUpdateAsUser : ExpectUpdate
    {

    }

    public class ExpectUpdateAsBasePermission : ExpectUpdate
    {

    }

    public class ExpectUpdateAsUIVersion : ExpectUpdate
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace FirstREST.RequestObjects
{
    public class EditArtigoData
    {
        public String fieldToEdit { get; set; }
        public String idOfProduct { get; set; }
        public String valueToSet { get; set; }

        public override string ToString()
        {
            return "[ Field To Edit: " + fieldToEdit + " \n Id of Product: " + idOfProduct + " \n New Value of Field: " + valueToSet + " ]";
        }

        public bool isFieldNull()
        {
            if (fieldToEdit == null || idOfProduct == null || valueToSet == null)
                return true;
            else {
                Debug.Write("\n A Filed is NULL\n");
                return false;
            }
        }
    }
}
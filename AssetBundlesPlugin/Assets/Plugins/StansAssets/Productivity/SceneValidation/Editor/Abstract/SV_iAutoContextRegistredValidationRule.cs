using System;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Productivity.SceneValidator
{

    /// <summary>
    /// This interface should be used if you want to register your validation rules
    /// without using the validation editor.
    /// All validation rules that are implement this interface will be registred on scripts load.
    /// </summary>
    public interface SV_iAutoContextRegistredValidationRule : SV_iContextValidationRule
    {

    }
}
using System;
using UnityEngine;

namespace ES3Types
{
    [UnityEngine.Scripting.Preserve]
    [ES3PropertiesAttribute()]
    public class ES3UserType_Animator : ES3ComponentType
    {
        public static ES3Type Instance = null;

        public ES3UserType_Animator() : base(typeof(UnityEngine.Animator)) { Instance = this; priority = 1; }


        protected override void WriteComponent(object obj, ES3Writer writer)
        {
            var instance = (UnityEngine.Animator)obj;

            foreach (var parameter in instance.parameters)
            {
                switch (parameter.type)
                {
                    case AnimatorControllerParameterType.Float:
                        var value = instance.GetFloat(parameter.name);
                        writer.WriteProperty<float>("Anim_Float_" + parameter.name, value );
                        break;
                    case AnimatorControllerParameterType.Int:
                        writer.WriteProperty<int>("Anim_Int_" + parameter.name, instance.GetInteger(parameter.name));
                        break;
                    case AnimatorControllerParameterType.Bool:
                        writer.WriteProperty<bool>("Anim_Bool_" + parameter.name, instance.GetBool(parameter.name));
                        break;
                    case AnimatorControllerParameterType.Trigger:                        
                        break;
                    default:
                        break;
                }
            }


        }

        protected override void ReadComponent<T>(ES3Reader reader, object obj)
        {
            var instance = (UnityEngine.Animator)obj;
            foreach (string propertyName in reader.Properties)
            {
                if (propertyName.StartsWith("Anim_"))
                {
                    var animName = propertyName.Replace("Anim_", "");
                    if (animName.StartsWith("Float_"))
                    {
                        var value = reader.Read<float>();
                        instance.SetFloat(animName.Replace("Float_", ""), value);
                    }
                    else if (animName.StartsWith("Bool_"))
                    {
                        instance.SetBool(animName.Replace("Bool_", ""), reader.Read<bool>());
                    }
                    else if (animName.StartsWith("Int_"))
                    {
                        instance.SetInteger(animName.Replace("Int_", ""), reader.Read<int>());
                    }
                    else
                    {
                        reader.Skip();
                    }
                }                
                else
                {
                    switch (propertyName)
                    {
                        default:
                            reader.Skip();
                            break;
                    }
                }
            }
        }
    }


    public class ES3UserType_AnimatorArray : ES3ArrayType
    {
        public static ES3Type Instance;

        public ES3UserType_AnimatorArray() : base(typeof(UnityEngine.Animator[]), ES3UserType_Animator.Instance)
        {
            Instance = this;
        }
    }
}
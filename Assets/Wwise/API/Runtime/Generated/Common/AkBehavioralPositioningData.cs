#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.2
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------
public class AkBehavioralPositioningData : global::System.IDisposable {
  private global::System.IntPtr swigCPtr;
  protected bool swigCMemOwn;
  internal AkBehavioralPositioningData(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }
  internal static global::System.IntPtr getCPtr(AkBehavioralPositioningData obj) {
    return (obj == null) ? global::System.IntPtr.Zero : obj.swigCPtr;
  }
  internal virtual void setCPtr(global::System.IntPtr cPtr) {
    Dispose();
    swigCPtr = cPtr;
  }
  ~AkBehavioralPositioningData() {
    Dispose(false);
  }
  public void Dispose() {
    Dispose(true);
    global::System.GC.SuppressFinalize(this);
  }
  protected virtual void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          AkSoundEnginePINVOKE.CSharp_delete_AkBehavioralPositioningData(swigCPtr);
        }
        swigCPtr = global::System.IntPtr.Zero;
      }
      global::System.GC.SuppressFinalize(this);
    }
  }
  public AkBehavioralPositioningData() : this(AkSoundEnginePINVOKE.CSharp_new_AkBehavioralPositioningData(), true) {
  }
  public float center { set { AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_center_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_center_get(swigCPtr); } 
  }
  public float panLR { set { AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_panLR_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_panLR_get(swigCPtr); } 
  }
  public float panBF { set { AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_panBF_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_panBF_get(swigCPtr); } 
  }
  public float panDU { set { AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_panDU_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_panDU_get(swigCPtr); } 
  }
  public float panSpatMix { set { AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_panSpatMix_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_panSpatMix_get(swigCPtr); } 
  }
  public Ak3DSpatializationMode spatMode { set { AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_spatMode_set(swigCPtr, (int)value); }  get { return (Ak3DSpatializationMode)AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_spatMode_get(swigCPtr); } 
  }
  public AkSpeakerPanningType panType { set { AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_panType_set(swigCPtr, (int)value); }  get { return (AkSpeakerPanningType)AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_panType_get(swigCPtr); } 
  }
  public bool enableHeightSpread { set { AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_enableHeightSpread_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkBehavioralPositioningData_enableHeightSpread_get(swigCPtr); } 
  }
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.

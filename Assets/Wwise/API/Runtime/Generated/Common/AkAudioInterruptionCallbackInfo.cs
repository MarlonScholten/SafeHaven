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
public class AkAudioInterruptionCallbackInfo : global::System.IDisposable {
  private global::System.IntPtr swigCPtr;
  protected bool swigCMemOwn;
  internal AkAudioInterruptionCallbackInfo(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }
  internal static global::System.IntPtr getCPtr(AkAudioInterruptionCallbackInfo obj) {
    return (obj == null) ? global::System.IntPtr.Zero : obj.swigCPtr;
  }
  internal virtual void setCPtr(global::System.IntPtr cPtr) {
    Dispose();
    swigCPtr = cPtr;
  }
  ~AkAudioInterruptionCallbackInfo() {
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
          AkSoundEnginePINVOKE.CSharp_delete_AkAudioInterruptionCallbackInfo(swigCPtr);
        }
        swigCPtr = global::System.IntPtr.Zero;
      }
      global::System.GC.SuppressFinalize(this);
    }
  }
  public bool bEnterInterruption { get { return AkSoundEnginePINVOKE.CSharp_AkAudioInterruptionCallbackInfo_bEnterInterruption_get(swigCPtr); } 
  }
  public AkAudioInterruptionCallbackInfo() : this(AkSoundEnginePINVOKE.CSharp_new_AkAudioInterruptionCallbackInfo(), true) {
  }
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.

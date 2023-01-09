/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_MENU_MUSIC = 2228153899U;
        static const AkUniqueID PLAY_MENUSFX1 = 2553873617U;
        static const AkUniqueID PLAY_MENUSFX2 = 2553873618U;
        static const AkUniqueID STOP_MENU_MUSIC = 3945748993U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace STEALTHSTATE
        {
            static const AkUniqueID GROUP = 2413224833U;

            namespace STATE
            {
                static const AkUniqueID ALERTED = 250639526U;
                static const AkUniqueID CAUTION = 1777167952U;
                static const AkUniqueID HIDDEN = 3621873013U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace STEALTHSTATE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace FOOTSTEPS
        {
            static const AkUniqueID GROUP = 2385628198U;

            namespace SWITCH
            {
                static const AkUniqueID CONCRETE = 841620460U;
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID GRAVEL = 2185786256U;
            } // namespace SWITCH
        } // namespace FOOTSTEPS

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID FOOTSTEPS_VOLUME = 960544891U;
        static const AkUniqueID MUSIC_VOLUME = 1006694123U;
        static const AkUniqueID SFX_VOLUME = 1564184899U;
        static const AkUniqueID VOICE_VOLUME = 3538560642U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAIN = 3161908922U;
        static const AkUniqueID MENU = 2607556080U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBIENT = 77978275U;
        static const AkUniqueID FOOTSTEPS = 2385628198U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SFX = 393239870U;
        static const AkUniqueID VOICE_OVER = 372659017U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__

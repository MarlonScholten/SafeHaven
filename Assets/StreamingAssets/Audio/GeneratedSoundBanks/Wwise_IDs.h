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
        static const AkUniqueID PLAY_FOOTSTEPS = 3854155799U;
        static const AkUniqueID PLAY_MENU_MUSIC = 2228153899U;
        static const AkUniqueID PLAY_PARAMETERTEST = 4229740275U;
        static const AkUniqueID PLAY_PROXLAMP_DRAFT1_CLICK = 2579192193U;
        static const AkUniqueID PLAY_PROXLAMP_DRAFT1_V2 = 1416161311U;
        static const AkUniqueID PLAY_STEALTH_MUSIC = 674798631U;
        static const AkUniqueID PLAY_TESTSOUND_LOOP = 2024687188U;
        static const AkUniqueID PLAY_TESTSOUND_SINGLE = 4206196186U;
        static const AkUniqueID PLAY_WIND_PLACEHOLDER = 3498798050U;
        static const AkUniqueID STOP_MENU_MUSIC = 3945748993U;
        static const AkUniqueID STOP_PARAMETERTEST = 2756796529U;
        static const AkUniqueID STOP_PROXLAMP_DRAFT1_V2 = 2594172909U;
        static const AkUniqueID STOP_STEALTH_MUSIC = 3876103401U;
        static const AkUniqueID STOP_TESTSOUND_LOOP = 3467245854U;
        static const AkUniqueID STOP_TESTSOUND_SINGLE = 25015968U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace STATETEST
        {
            static const AkUniqueID GROUP = 3275448284U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID OFF = 930712164U;
                static const AkUniqueID ON = 1651971902U;
            } // namespace STATE
        } // namespace STATETEST

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
        static const AkUniqueID TESTPARAMETER = 2010788230U;
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
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__

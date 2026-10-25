using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepController : MonoBehaviour
{
    private ManiacMove maniacMove;
    [SerializeField] private AudioClip stepCommon, stepMuted, stepTop, stepTrample, stepBottom, stepMutedBottom;
    private void Awake()
    {
        maniacMove = GetComponent<ManiacMove>();
    }
    public void Set_AnotherStep()
    {// room 1 - start
        if ((maniacMove.move_id == 7 || maniacMove.move_id == 5)
            && maniacMove.current_room == 1)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTop)
            {
                maniacMove.step.clip = stepTop;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 6)
            && maniacMove.current_room == 1)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTrample)
            {
                maniacMove.step.clip = stepTrample;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 8 || maniacMove.move_id == 9 || maniacMove.move_id == 10 || maniacMove.move_id == 11 || maniacMove.move_id == 12)
            && maniacMove.current_room == 1)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepMuted)
            {
                maniacMove.step.clip = stepMuted;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 2 || maniacMove.move_id == 3)
            && maniacMove.current_room == 1)
        {
            maniacMove.step.spatialBlend = 0.6f;
            if (maniacMove.step.clip != stepCommon)
            {
                maniacMove.step.clip = stepCommon;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 4)
            && maniacMove.current_room == 1)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepBottom)
            {
                maniacMove.step.clip = stepBottom;
                maniacMove.step.Play();
            }
        }
        // room 1 - end


        // room 2 - start
        if ((maniacMove.move_id == 7 || maniacMove.move_id == 6)
            && maniacMove.current_room == 2)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTop)
            {
                maniacMove.step.clip = stepTop;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 5)
            && maniacMove.current_room == 2)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTrample)
            {
                maniacMove.step.clip = stepTrample;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 8 || maniacMove.move_id == 9 || maniacMove.move_id == 10 || maniacMove.move_id == 11 || maniacMove.move_id == 12)
            && maniacMove.current_room == 2)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepMuted)
            {
                maniacMove.step.clip = stepMuted;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 2 || maniacMove.move_id == 1)
            && maniacMove.current_room == 2)
        {
            maniacMove.step.spatialBlend = 0.6f;
            if (maniacMove.step.clip != stepCommon)
            {
                maniacMove.step.clip = stepCommon;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 4)
            && maniacMove.current_room == 2)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepBottom)
            {
                maniacMove.step.clip = stepBottom;
                maniacMove.step.Play();
            }
        }
        // room 2 - end

        // room 3 - start
        if ((maniacMove.move_id == 7 || maniacMove.move_id == 6 || maniacMove.move_id == 5)
            && maniacMove.current_room == 3)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTop)
            {
                maniacMove.step.clip = stepTop;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 8 || maniacMove.move_id == 9 || maniacMove.move_id == 10 || maniacMove.move_id == 11 || maniacMove.move_id == 12)
            && maniacMove.current_room == 3)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepMuted)
            {
                maniacMove.step.clip = stepMuted;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 1 || maniacMove.move_id == 2)
            && maniacMove.current_room == 3)
        {
            maniacMove.step.spatialBlend = 0.6f;
            if (maniacMove.step.clip != stepCommon)
            {
                maniacMove.step.clip = stepCommon;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 4)
            && maniacMove.current_room == 3)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepBottom)
            {
                maniacMove.step.clip = stepBottom;
                maniacMove.step.Play();
            }
        }
        // room 3 - end

        // room 4 - start
        if ((maniacMove.move_id == 1 || maniacMove.move_id == 2)
            && maniacMove.current_room == 4)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTop)
            {
                maniacMove.step.clip = stepTop;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 3)
            && maniacMove.current_room == 4)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTrample)
            {
                maniacMove.step.clip = stepTrample;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 8 || maniacMove.move_id == 9 || maniacMove.move_id == 10 || maniacMove.move_id == 11 || maniacMove.move_id == 12 || maniacMove.move_id == 5 || maniacMove.move_id == 6 || maniacMove.move_id == 7)
            && maniacMove.current_room == 4)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepMuted)
            {
                maniacMove.step.clip = stepMuted;
                maniacMove.step.Play();
            }
        }
        // room 4 - end

        // room 5 - start
        if ((maniacMove.move_id == 8 || maniacMove.move_id == 9 || maniacMove.move_id == 10)
            && maniacMove.current_room == 5)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTop)
            {
                maniacMove.step.clip = stepTop;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 11 || maniacMove.move_id == 12)
            && maniacMove.current_room == 5)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTrample)
            {
                maniacMove.step.clip = stepTrample;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 6 || maniacMove.move_id == 7)
            && maniacMove.current_room == 5)
        {
            maniacMove.step.spatialBlend = 0.6f;
            if (maniacMove.step.clip != stepCommon)
            {
                maniacMove.step.clip = stepCommon;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 1 || maniacMove.move_id == 2 || maniacMove.move_id == 3 || maniacMove.move_id == 4)
            && maniacMove.current_room == 5)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepBottom)
            {
                maniacMove.step.clip = stepBottom;
                maniacMove.step.Play();
            }
        }
        // room 5 - end

        // room 6 - start
        if ((maniacMove.move_id == 8 || maniacMove.move_id == 9 || maniacMove.move_id == 11 || maniacMove.move_id == 12)
            && maniacMove.current_room == 6)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTop)
            {
                maniacMove.step.clip = stepTop;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 10)
            && maniacMove.current_room == 6)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTrample)
            {
                maniacMove.step.clip = stepTrample;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 5 || maniacMove.move_id == 7)
            && maniacMove.current_room == 6)
        {
            maniacMove.step.spatialBlend = 0.6f;
            if (maniacMove.step.clip != stepCommon)
            {
                maniacMove.step.clip = stepCommon;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 1 || maniacMove.move_id == 2 || maniacMove.move_id == 3 || maniacMove.move_id == 4)
            && maniacMove.current_room == 6)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepBottom)
            {
                maniacMove.step.clip = stepBottom;
                maniacMove.step.Play();
            }
        }
        // room 6 - end

        // room 7 - start
        if ((maniacMove.move_id == 10 || maniacMove.move_id == 11 || maniacMove.move_id == 12)
            && maniacMove.current_room == 7)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTop)
            {
                maniacMove.step.clip = stepTop;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 8 || maniacMove.move_id == 9)
            && maniacMove.current_room == 7)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepTrample)
            {
                maniacMove.step.clip = stepTrample;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 6 || maniacMove.move_id == 5)
            && maniacMove.current_room == 7)
        {
            maniacMove.step.spatialBlend = 0.6f;
            if (maniacMove.step.clip != stepCommon)
            {
                maniacMove.step.clip = stepCommon;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 1 || maniacMove.move_id == 2 || maniacMove.move_id == 3 || maniacMove.move_id == 4)
            && maniacMove.current_room == 7)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepBottom)
            {
                maniacMove.step.clip = stepBottom;
                maniacMove.step.Play();
            }
        }
        // room 7 - end

        // room 8 - start
        if ((maniacMove.move_id == 4 || maniacMove.move_id == 1 || maniacMove.move_id == 2 || maniacMove.move_id == 3)
            && maniacMove.current_room == 8)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepMutedBottom)
            {
                maniacMove.step.clip = stepMutedBottom;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 9 || maniacMove.move_id == 10 || maniacMove.move_id == 11 || maniacMove.move_id == 12)
            && maniacMove.current_room == 8)
        {
            maniacMove.step.spatialBlend = 0.6f;
            if (maniacMove.step.clip != stepCommon)
            {
                maniacMove.step.clip = stepCommon;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 5 || maniacMove.move_id == 6 || maniacMove.move_id == 7)
            && maniacMove.current_room == 8)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepBottom)
            {
                maniacMove.step.clip = stepBottom;
                maniacMove.step.Play();
            }
        }
        // room 8 - end

        // room 9 - start
        if ((maniacMove.move_id == 1 || maniacMove.move_id == 2 || maniacMove.move_id == 3 || maniacMove.move_id == 4)
            && maniacMove.current_room == 9)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepMutedBottom)
            {
                maniacMove.step.clip = stepMutedBottom;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 8 || maniacMove.move_id == 10 || maniacMove.move_id == 11 || maniacMove.move_id == 12)
            && maniacMove.current_room == 9)
        {
            maniacMove.step.spatialBlend = 0.6f;
            if (maniacMove.step.clip != stepCommon)
            {
                maniacMove.step.clip = stepCommon;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 5 || maniacMove.move_id == 6 || maniacMove.move_id == 7)
            && maniacMove.current_room == 9)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepBottom)
            {
                maniacMove.step.clip = stepBottom;
                maniacMove.step.Play();
            }
        }
        // room 9 - end

        // room 10 - start
        if ((maniacMove.move_id == 1 || maniacMove.move_id == 2 || maniacMove.move_id == 3 || maniacMove.move_id == 4)
            && maniacMove.current_room == 10)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepMutedBottom)
            {
                maniacMove.step.clip = stepMutedBottom;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 8 || maniacMove.move_id == 9 || maniacMove.move_id == 11 || maniacMove.move_id == 12)
            && maniacMove.current_room == 10)
        {
            maniacMove.step.spatialBlend = 0.6f;
            if (maniacMove.step.clip != stepCommon)
            {
                maniacMove.step.clip = stepCommon;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 5 || maniacMove.move_id == 6 || maniacMove.move_id == 7)
            && maniacMove.current_room == 10)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepBottom)
            {
                maniacMove.step.clip = stepBottom;
                maniacMove.step.Play();
            }
        }
        // room 10 - end

        // room 11 - start
        if ((maniacMove.move_id == 1 || maniacMove.move_id == 2 || maniacMove.move_id == 3 || maniacMove.move_id == 4)
            && maniacMove.current_room == 11)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepMutedBottom)
            {
                maniacMove.step.clip = stepMutedBottom;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 8 || maniacMove.move_id == 9 || maniacMove.move_id == 10 || maniacMove.move_id == 12)
            && maniacMove.current_room == 11)
        {
            maniacMove.step.spatialBlend = 0.6f;
            if (maniacMove.step.clip != stepCommon)
            {
                maniacMove.step.clip = stepCommon;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 5 || maniacMove.move_id == 6 || maniacMove.move_id == 7)
            && maniacMove.current_room == 11)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepBottom)
            {
                maniacMove.step.clip = stepBottom;
                maniacMove.step.Play();
            }
        }
        // room 11 - end

        // room 12 - start
        if ((maniacMove.move_id == 1 || maniacMove.move_id == 2 || maniacMove.move_id == 3 || maniacMove.move_id == 4)
            && maniacMove.current_room == 12)
        {
            maniacMove.step.spatialBlend = 0.3f;
            if (maniacMove.step.clip != stepMutedBottom)
            {
                maniacMove.step.clip = stepMutedBottom;
                maniacMove.step.Play();
            }
        }
        else if ((maniacMove.move_id == 8 || maniacMove.move_id == 9 || maniacMove.move_id == 10 || maniacMove.move_id == 11)
            && maniacMove.current_room == 12)
        {
            maniacMove.step.spatialBlend = 1f;
            if (maniacMove.step.clip != stepCommon)
            {
                {
                    maniacMove.step.clip = stepCommon;
                    maniacMove.step.Play();
                }
            }
            else if ((maniacMove.move_id == 5 || maniacMove.move_id == 6 || maniacMove.move_id == 7)
                && maniacMove.current_room == 12)
            {
                maniacMove.step.spatialBlend = 0.3f;
                if (maniacMove.step.clip != stepBottom)
                {
                    maniacMove.step.clip = stepBottom;
                    maniacMove.step.Play();
                }
            }
            // room 12 - end
        }
    }
        public void Set_StereoPan()
        {
            switch (maniacMove.current_room)
            {
                case var _ when maniacMove.current_room == 1 && maniacMove.move_id == 2:
                    maniacMove.step.panStereo = 0.5f;
                    break;
                case var _ when maniacMove.current_room == 1 && maniacMove.move_id == 3:
                    maniacMove.step.panStereo = 0.75f;
                    break;
                case var _ when maniacMove.current_room == 1 && maniacMove.move_id == 4:
                    maniacMove.step.panStereo = 1f;
                    break;


                case var _ when maniacMove.current_room == 2 && maniacMove.move_id == 1:
                    maniacMove.step.panStereo = -0.5f;
                    break;
                case var _ when maniacMove.current_room == 2 && maniacMove.move_id == 3:
                    maniacMove.step.panStereo = 0.5f;
                    break;
                case var _ when maniacMove.current_room == 2 && maniacMove.move_id == 4:
                    maniacMove.step.panStereo = 0.75f;
                    break;


                case var _ when maniacMove.current_room == 3 && maniacMove.move_id == 1:
                    maniacMove.step.panStereo = -0.75f;
                    break;
                case var _ when maniacMove.current_room == 3 && maniacMove.move_id == 2:
                    maniacMove.step.panStereo = -0.5f;
                    break;

                //Этаж 2

                case var _ when maniacMove.current_room == 5 && maniacMove.move_id == 6:
                    maniacMove.step.panStereo = -0.5f;
                    break;
                case var _ when maniacMove.current_room == 5 && maniacMove.move_id == 7:
                    maniacMove.step.panStereo = -0.75f;
                    break;

                case var _ when maniacMove.current_room == 6 && maniacMove.move_id == 5:
                    maniacMove.step.panStereo = 0.5f;
                    break;
                case var _ when maniacMove.current_room == 6 && maniacMove.move_id == 7:
                    maniacMove.step.panStereo = -0.5f;
                    break;

                case var _ when maniacMove.current_room == 7 && maniacMove.move_id == 5:
                    maniacMove.step.panStereo = 0.5f;
                    break;
                case var _ when maniacMove.current_room == 7 && maniacMove.move_id == 6:
                    maniacMove.step.panStereo = 0.75f;
                    break;

                //Этаж 3

                case var _ when maniacMove.current_room == 8 && maniacMove.move_id == 10:
                    maniacMove.step.panStereo = 0.5f;
                    break;
                case var _ when maniacMove.current_room == 8 && maniacMove.move_id == 11:
                    maniacMove.step.panStereo = 0.75f;
                    break;
                case var _ when maniacMove.current_room == 8 && maniacMove.move_id == 12:
                    maniacMove.step.panStereo = 1f;
                    break;

                case var _ when maniacMove.current_room == 10 && maniacMove.move_id == 9:
                    maniacMove.step.panStereo = -0.5f;
                    break;
                case var _ when maniacMove.current_room == 10 && maniacMove.move_id == 8:
                    maniacMove.step.panStereo = -0.5f;
                    break;
                case var _ when maniacMove.current_room == 10 && maniacMove.move_id == 11:
                    maniacMove.step.panStereo = 0.5f;
                    break;
                case var _ when maniacMove.current_room == 10 && maniacMove.move_id == 12:
                    maniacMove.step.panStereo = 0.75f;
                    break;

                case var _ when maniacMove.current_room == 11 && maniacMove.move_id == 9:
                    maniacMove.step.panStereo = -0.75f;
                    break;
                case var _ when maniacMove.current_room == 11 && maniacMove.move_id == 8:
                    maniacMove.step.panStereo = -0.75f;
                    break;
                case var _ when maniacMove.current_room == 11 && maniacMove.move_id == 10:
                    maniacMove.step.panStereo = -0.5f;
                    break;
                case var _ when maniacMove.current_room == 11 && maniacMove.move_id == 12:
                    maniacMove.step.panStereo = 0.5f;
                    break;

                case var _ when maniacMove.current_room == 12 && maniacMove.move_id == 9:
                    maniacMove.step.panStereo = -1f;
                    break;
                case var _ when maniacMove.current_room == 12 && maniacMove.move_id == 8:
                    maniacMove.step.panStereo = -1f;
                    break;
                case var _ when maniacMove.current_room == 12 && maniacMove.move_id == 10:
                    maniacMove.step.panStereo = -0.75f;
                    break;
                case var _ when maniacMove.current_room == 12 && maniacMove.move_id == 11:
                    maniacMove.step.panStereo = -0.5f;
                    break;


                default:
                    maniacMove.step.panStereo = 0f;
                    break;
            }
        }
    }

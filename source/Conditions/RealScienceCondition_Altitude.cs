﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace RealScience.Conditions
{
    public class RealScienceCondition_Altitude : RealScienceCondition
    {
        // common properties
        public string conditionType = "Altitude";
        public bool restriction = false;
        public string exclusion = "";
        public float dataRateModifier = 1f;
        public float maximumDataModifier = 1f;
        public float maximumDataBonus = 0f;
        // specific properties
        public float altitudeMin = 0f;
        public float altitudeMax = float.MaxValue;

        protected string tooltip;

        public override float DataRateModifier
        {
            get { return dataRateModifier; }
        }
        public override float MaximumDataModifier
        {
            get { return maximumDataModifier; }
        }
        public override float MaximumDataBonus
        {
            get { return maximumDataBonus; }
        }
        public override bool IsRestriction
        {
            get { return restriction; }
        }
        public override string Exclusion
        {
            get { return exclusion; }
        }
        public override string Tooltip
        {
            get { return tooltip; }
        }
        public override string Name
        {
            get { return conditionType; }
        }

		public override EvalState Evaluate(Part part, float deltaTime, ExperimentState state)
        {
            bool valid = true;
            tooltip = "\nAltitude Condition";
            if (restriction)
            {
                if (exclusion.ToLower() == "reset")
                    tooltip += "\nThe following condition must <b>not</b> be met.  If they are the experiment will be <b>reset</b>.";
                else if (exclusion.ToLower() == "fail")
                    tooltip += "\nThe following condition must <b>not</b> be met.  If they are, the experiment will <b>fail</b>.";
                else
                    tooltip += "\nThe following condition must <b>not</b> be met.";
            }
            else
                tooltip += "\nThe following condition must be met.";

            float altitude = FlightGlobals.getAltitudeAtPos(part.transform.position);
            tooltip += String.Format("\nAltitude between <b>{0:F2}</b> and <b>{1:F2}</b>.  Currently <b>{2:F2}</b>", altitudeMin, altitudeMax, altitude);
            if (altitude >= altitudeMin && altitude <= altitudeMax)
                valid = true;
            else
                valid = false;

            if (!restriction)
            {
                if (valid)
                    return EvalState.VALID;
                else
                    return EvalState.INVALID;
            }
            else
            {
                if (!valid)
                    return EvalState.VALID;
                else
                {
                    if (exclusion.ToLower() == "reset")
                        return EvalState.RESET;
                    else if (exclusion.ToLower() == "fail")
                        return EvalState.FAILED;
                    else
                        return EvalState.INVALID;
                }
            }
        }

        public override void Load(ConfigNode node)
        {
            // Load common properties
            if (node.HasValue("conditionType"))
                conditionType = node.GetValue("conditionType");
            if (node.HasValue("restriction"))
            {
                try
                {
                    restriction = bool.Parse(node.GetValue("restriction"));
                }
                catch (FormatException)
                {
                    restriction = false;
                }
            }
            if (node.HasValue("dataRateModifier"))
            {
                try
                {
                    dataRateModifier = float.Parse(node.GetValue("dataRateModifier"));
                }
                catch (FormatException)
                {
                    dataRateModifier = 1f;
                }
            }
            // Load specific properties
            if (node.HasValue("altitudeMin"))
            {
                try
                {
                    altitudeMin = float.Parse(node.GetValue("altitudeMin"));
                }
                catch (FormatException)
                {
                    altitudeMin = 0f;
                }

            }
            if (node.HasValue("altitudeMax"))
            {
                try
                {
                    altitudeMax = float.Parse(node.GetValue("altitudeMax"));
                }
                catch (FormatException)
                {
                    altitudeMax = float.MaxValue;
                }

            }
        }
        public override void Save(ConfigNode node)
        {
            // Save common properties
            node.AddValue("conditionType", conditionType);
            node.AddValue("restriction", restriction);
            node.AddValue("dataRateModifier", dataRateModifier);
            // Save specific properties
            node.AddValue("altitudeMin", altitudeMin);
            node.AddValue("altitudeMax", altitudeMax);
        }
    }
}

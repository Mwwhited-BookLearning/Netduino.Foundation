using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Controllers.PID
{
    public class IdealPidController : PidControllerBase
    {
        public override float CalculateControlOutput()
        {
            // init vars
            float control = 0.0f;
            var now = DateTime.Now;

            // time delta (how long since last calculation)
            var dt = now - _lastUpdateTime;
            // seconds is better than ticks to bring our calculations into perspective
            //var seconds = (float)(dt.Ticks / 10000 / 1000);
            var seconds = ((float)dt.Ticks / 10000f / 1000f);

            // if no time has passed, don't make any changes.
            if (dt.Ticks <= 0.0) return _lastControlOutputValue;

            // copy vars
            var input = ActualInput;
            var target = TargetInput;

            // calculate the error (how far we are from target)
            var error = target - input;
            //Debug.Print("Actual: " + ActualInput.ToString("N1") + ", Error: " + error.ToString("N1"));

            // calculate the proportional term
            var proportional = ProportionalComponent * error;
            //Debug.Print("Proportional: " + proportional.ToString("N2"));

            // calculate the integral
            _integral += error * seconds; // add to the integral history
            var integral = IntegralComponent * _integral; // calcuate the integral action

            // calculate the derivative (rate of change, slop of line) term
            var diff = error - _lastError / seconds;
            var derivative = DerivativeComponent * diff;

            // add the appropriate corrections
            control = proportional + integral + derivative;

            //
            //Debug.Print("PID Control (preclamp): " + control.ToString("N4"));

            // clamp
            if (control > OutputMax) control = OutputMax;
            if (control < OutputMin) control = OutputMin;

            //Debug.Print("PID Control (postclamp): " + control.ToString("N4"));

            if (OutputTuningInformation)
            {
                Debug.Print("SP+PV+PID+O," + target.ToString() + "," + input.ToString() + "," +
                    proportional.ToString() + "," + integral.ToString() + "," +
                    derivative.ToString() + "," + control.ToString());
            }

            // persist our state variables
            _lastControlOutputValue = control;
            _lastError = error;
            _lastUpdateTime = now;

            return control;
        }

    }
}

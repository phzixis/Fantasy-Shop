using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock
{
    int _time;
    public int minutesTotal {
        get { return _time; }
    }
    public int minutes {
        get { return _time % 60; }
    }
    public int hours {
        get {return _time / 60; }
    }

    public Clock (int time) {
        _time = time;
    }

    public Clock (int hours, int minutes) {
        _time = hours * 60 + minutes;
    }

    public Clock (Clock time) {
        _time = time.minutesTotal;
    }

    public static Clock operator + (Clock time, int mins) {
        return new Clock(time.minutesTotal + mins);
    }

    public static Clock operator + (Clock time1, Clock time2) {
        return new Clock(time1.minutesTotal + time2.minutesTotal);
    }

    public static Clock operator - (Clock time1, Clock time2) {
        return new Clock(Mathf.Abs(time1.minutesTotal - time2.minutesTotal));
    }

    public static Clock operator - (Clock time, int mins) {
        int ans = time.minutesTotal - mins;
        if (ans > 0) {
            return new Clock(ans);
        } else {
            return new Clock(0);
        }
    }

    public static Clock operator * (Clock time1, Clock time2) {
        return new Clock(time1.minutesTotal * time2.minutesTotal);
    }

    public static Clock operator * (Clock time, int mins) {
        return new Clock(time.minutesTotal * mins);
    }
}

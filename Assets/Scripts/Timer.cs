struct Timer
{
    float duration;
    Timer(float duration) { this.duration = duration; }
    bool IsDone() { return duration <= 0; }
}

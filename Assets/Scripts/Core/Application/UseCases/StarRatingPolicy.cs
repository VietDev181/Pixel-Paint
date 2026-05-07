public class StarRatingPolicy
{
    public const float ThreeStarThreshold = 1f;
    public const float TwoStarThreshold = 0.9f;
    public const float OneStarThreshold = 0.75f;

    public int Rate(float accuracy)
    {
        if (accuracy >= ThreeStarThreshold) return 3;
        if (accuracy >= TwoStarThreshold) return 2;
        if (accuracy >= OneStarThreshold) return 1;
        return 0;
    }
}

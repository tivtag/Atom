
namespace Atom.Story.Specifications.StoryboardSpecifications
{
    using Machine.Specifications;

    [Subject( typeof( Storyboard ) )]
    public class when_newly_created
    {
        static Storyboard storyboard;

        Because of = () =>
            storyboard = new Storyboard();

        It should_contain_no_timelines = () =>
            storyboard.TimelineCount.ShouldEqual(0);
    }
}

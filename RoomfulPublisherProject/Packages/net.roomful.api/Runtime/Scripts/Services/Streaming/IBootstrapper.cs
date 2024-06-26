using System.Collections.Generic;
using net.roomful.api.props;

namespace net.roomful.api.karaoke
{
    public readonly struct SubjectBootstrapCompletedArgs
    {
        public readonly IKaraokeSubject Subject;

        public SubjectBootstrapCompletedArgs(IKaraokeSubject subject) {
            Subject = subject;
        }
    }
    
    public readonly struct SubjectBeforeDisposeArgs
    {
        public readonly IKaraokeSubject Subject;
        
        public SubjectBeforeDisposeArgs(IKaraokeSubject subject) {
            Subject = subject;
        }
    }
    
    public delegate void SubjectBootstrapCompleted(SubjectBootstrapCompletedArgs args);
    public delegate void SubjectBeforeDispose(SubjectBeforeDisposeArgs args);
    
    public interface IBootstrapper
    {
        event SubjectBootstrapCompleted OnSubjectBootstrapCompleted;
        event SubjectBeforeDispose OnSubjectBeforeDispose;
        
        IEnumerable<IKaraokeSubject> Subjects { get; }
        void Bootstrap(IPropComponent component);
        void Dispose(IPropComponent component);
    }
}

Design Decisions:

This project uses a concrete repository pattern. There has been no attempt made to create a generic repository because the
code base would become overly complex in trying to achieve what entity framework does in an implementation agnostic way.

A Unit Of Work pattern has also been implemented in order to ensure ACID operations. There is the possibility of several different
syntaxes to enforce UoW. An approach using scopes has been chosen as I feel that syntax is cleaner than a syntax requiring operations
to be explicitly tracked. Eg a UnitOfWork concrete that is instantiated with repositories to commit in the constructor.

The entities have been kept clean of any EF related attributes in order to keep the models as clean as possible and agnostic
of any implementation details. Instead the fluent API has been used.

You may note that there is no default implementation of IRepository - this is because the repositories should be based on
entity model aggregate roots and with that in mind we cannot know what an Add would do, or what data it would persist.
This is another reason the generic repository pattern was not chosen. There is however an interface of several general methods
to implement which I feel would save time and improve consistency in the long run.
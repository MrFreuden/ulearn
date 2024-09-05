using System;
using System.Text;

namespace hashes;

public class GhostsTask :
    IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
    IMagic
{
    private Vector _vector;
    private Segment _segment;
    private Document _document;
    private Cat _cat;
    private Robot _robot;
    private byte[] _content;

    public void DoMagic()
    {
        _vector ??= ((IFactory<Vector>)this).Create();
        _vector.Add(new Vector(2.2, 2.2));

        _segment ??= ((IFactory<Segment>)this).Create();
        _segment.Start.Add(_vector);

        _document ??= ((IFactory<Document>)this).Create();
        _content[0] = 111;

        _cat ??= ((IFactory<Cat>)this).Create();
        _cat.Rename("AAA");

        _robot ??= ((IFactory<Robot>)this).Create();
        Robot.BatteryCapacity *= DateTime.Now.Month;
    }

    Vector IFactory<Vector>.Create()
    {
        return _vector ??= new Vector(1.2, 1.1);
    }

    Segment IFactory<Segment>.Create()
    {
        return _segment ??= new Segment(new Vector(1.1, 1.1), new Vector(2.1, 2.1));
    }

    Document IFactory<Document>.Create()
    {
        return _document ??= new Document("Test", Encoding.UTF8, _content ??= new byte[1]);
    }

    Cat IFactory<Cat>.Create()
    {
        return _cat ??= new Cat("Test", "TestBreed", DateTime.Now);
    }

    Robot IFactory<Robot>.Create()
    {
        return _robot ??= new Robot("1", 2);
    }
}
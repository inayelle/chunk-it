using System.Numerics;
using System.Runtime.CompilerServices;
using ChunkIt.Common.Extensions;

namespace ChunkIt.Partitioning.Gear;

public sealed class GearTable
{
    private readonly ulong[] _table;

    internal GearTable(ulong[] table)
    {
        _table = table;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Fingerprint(ref ulong fingerprint, ref readonly byte value)
    {
        unchecked
        {
            fingerprint = (fingerprint << 1) + _table[value];
        }
    }

    public static GearTable Random(Random random)
    {
        var seed = random.NextUInt64();

        return RandomGearTable.Create(seed);
    }

    public static GearTable Predefined(int rotations, int tableIndex = 0)
    {
        return PredefinedGearTable.Create(rotations, tableIndex);
    }
}

file static class RandomGearTable
{
    public static GearTable Create(ulong seed)
    {
        var table = new ulong[256];

        for (var i = 0; i < table.Length; i++)
        {
            table[i] = SplitMix64(ref seed);
        }

        return new GearTable(table);
    }

    private static ulong SplitMix64(ref ulong seed)
    {
        unchecked
        {
            seed += 0x9E3779B97F4A7C15UL;

            var z = seed;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;

            return z ^ (z >> 31);
        }
    }
}

file static class PredefinedGearTable
{
    private static readonly ulong[] Table0 =
    [
        0x651748f5a15f8222, 0xd6eda276c877d8ea, 0x66896ef9591b326b,
        0xcd97506b21370a12, 0x8c9c5c9acbeb2a05, 0xb8b9553ee17665ef,
        0x1784a989315b1de6, 0x947666c9c50df4bd, 0xb3f660ea7ff2d6a4,
        0xbcd6adb8d6d70eb5, 0xb0909464f9c63538, 0xe50e3e46a8e1b285,
        0x21ed7b80c0163ce0, 0xf209acd115f7b43b, 0xb8c9cb07eaf16a58,
        0xb60478aa97ba854c, 0x8fb213a0b5654c3d, 0x42e8e7bd9fb03710,
        0x737e3de60a90b54f, 0x9172885f5aa79c8b, 0x787faae7be109c36,
        0x86ad156f5274cb9f, 0x6ac0a8daa59ee1ab, 0x5e55bc229d5c618e,
        0xa54fb69a5f181d41, 0xc433d4cf44d8e974, 0xd9efe85b722e48a3,
        0x7a5e64f9ea3d9759, 0xba3771e13186015d, 0x5d468c5fad6ef629,
        0x96b1af02152ebfde, 0x63706f4aa70e0111, 0xe7a9169252de4749,
        0xf548d62570bc8329, 0xee639a9117e8c946, 0xd31b0f46f3ff6847,
        0xfed7938495624fc5, 0x1ef2271c5a28122e, 0x7fd8e0e95eac73ef,
        0x920558e0ee131d4c, 0xce2e67cb1034bcd1, 0x6f4b338d34b004ae,
        0x92f5e7271cf95c9a, 0x12e1305a9c558342, 0x1e30d88013ad77ae,
        0x09acc1a57bbb604e, 0xaf187082c6f56192, 0xd2e5d987f04ac6f0,
        0x3b22fca40423da70, 0x7dfba8ce699a9a87, 0xe8b15f90ea96bd2a,
        0xcda1a1089cc2cbe7, 0x72f70448459de898, 0x1ab992dbb61cd46e,
        0x912ad04becbb29da, 0x98c6bb3aa3ce09ed, 0x6373bd2e7a041f3a,
        0x1f98f28bd178c53a, 0xe6adbc82ba5d9f96, 0x7456da7d805cbe01,
        0xd673662dcc135eeb, 0xb299e26eaadcb311, 0x2c2582172f8114af,
        0xeded114d7f623da6, 0xb3462a0e623276e4, 0x3af752be3d34bfaa,
        0x1311ccc0a1855a89, 0x0812bbcecc92b2e4, 0x9974b5747289f2f5,
        0x3a030eff770f2026, 0x52462b2aa42a847a, 0x2beaa107d15a012b,
        0x0c0035e0fe073398, 0x4f2f9de2ac206766, 0x5dd51a617c291deb,
        0x1ac66905652cc03b, 0x11067b0947fc07a1, 0x02b5fcd96ad06d52,
        0x74244ec1aa2821fd, 0xf6089e32060e9439, 0xd8f076a33bcbf1a7,
        0x5162743c755d8d5e, 0x8d34fc683e4e3d06, 0x46efe9b21a0252a3,
        0x4631e8d0109c6145, 0xfdf7a14bc0223957, 0x750934b3d0b8bb1e,
        0x2ecd1b3efed5ddb9, 0x2bcbd89a83ccfbce, 0x3507c79e58dd5886,
        0x5476a67ecd4a772f, 0xaa0be3856dd76405, 0x22289a358a4dd421,
        0xf570433f14503ad1, 0x8a9f440251a722c3, 0x77dd711752b4398c,
        0xbbd9edf9c6160a31, 0xb94b59220b23f079, 0xfdca3d75d2f33ccf,
        0xb29452c460c9e977, 0xe89afe2dd4bf3b02, 0x47ec6f32c91bfee4,
        0x1aab5ec3445706b8, 0x588bf4fa55334006, 0xe2290ca1e29acd96,
        0x3c49e189f831c37c, 0x6448c973b5177498, 0x556a6e09ba158de7,
        0x90b25013a8d9a067, 0xa4f2f7a50c58e1c4, 0x5e765e871008700e,
        0x242f5ae7738327af, 0xc1e6a2819cc5a219, 0xcb48d801fd6a5449,
        0xa208de2301931383, 0xde3c143fe44e39b0, 0x6bb74b09c73e4133,
        0xb5b1ed1b63d54c11, 0x587567d454ce7716, 0xf47ddbc987cb0392,
        0x87b19254448f03f1, 0x985fd00ec372fafa, 0x64b92ba521aa46e4,
        0xce63f4013d587b0f, 0xa691ae698726030e, 0xeaefbf690264e9aa,
        0x68edd400523eb152, 0x35d9353aa1957c60, 0x2e2c2d7a9cb68385,
        0xfc7549edaf43bf9e, 0x48b2adb23026e2c7, 0x3777cb79a024bcf9,
        0x644128f7c184102d, 0x70189d3ca4390de9, 0x085fea7986d4cd34,
        0x6dbe7626c8457464, 0x9fa41cfa9c4265eb, 0xdaa163a641946463,
        0x02f5c4bd9efa2074, 0x783201871822c3c9, 0xb0dfec499202bce0,
        0x1f1c9c12d84dccab, 0x1596f8819f2ed68e, 0xb0352c3e9fc84468,
        0x24a6673db9122956, 0x84f5b9e60b274739, 0x7216b28a0b54ac46,
        0xc7789de20e9cdca4, 0x903db5d289dd6563, 0xce66a947f7033516,
        0x3677dbc62307b2ca, 0x8d8e9d5530eb46ac, 0x79c4bad281bd93e2,
        0x287d942042068c36, 0xde4b98e5464b6ad5, 0x612534b97d1d21bf,
        0xdf98659772d822a1, 0x93053df791aa6264, 0x2254a8a2d54528ba,
        0x2301164aeb69c43d, 0xf56863474ac2417f, 0x6136b73e1b75de42,
        0xc7c3bd487e06b532, 0x7232fbed1eb9be85, 0x36d60f0bd7909e43,
        0xe08cbf774a4ce1f2, 0xf75fbc0d97cb8384, 0xa5097e5af367637b,
        0x7bce2dcfa856dbb2, 0xfbfb729dd808c894, 0x3dc8eba10ad7112e,
        0xf2d1854eedce4928, 0xb705f5c1aebd2104, 0x78fa4d004417d956,
        0x9e5162660729f858, 0xda0bcd5eb9f91f0e, 0x748d1be11e06b362,
        0xf4c2be9a04547734, 0x6f2bcd7c88abdf9a, 0x50865dafdfd8a404,
        0x9d820665691728f0, 0x59fe7a56aa07118e, 0x4df1d768c23660ec,
        0xab6310b8edfb8c5e, 0x029b47623fc9ffe4, 0x50c2cca231374860,
        0x0561505a8dbbdc69, 0x8d07fe136de385f3, 0xc7fb6bb1731b1c1c,
        0x2496d1256f1fac7a, 0x79508cee90d84273, 0x09f51a2108676501,
        0x2ef72d3dc6a50061, 0xe4ad98f5792dd6d6, 0x69fa05e609ae7d33,
        0xf7f30a8b9ae54285, 0x04a2cb6a0744764b, 0xc4b0762f39679435,
        0x60401bc93ef6047b, 0x76f6aa76e23dbe0c, 0x8a209197811e39da,
        0x4489a9683fa03888, 0x2604ad5741a6f8d8, 0x7faa9e0c64a94532,
        0x0dbfee8cdae8f54e, 0x0a7c5885f0b76d4a, 0x55dfb1ac12e83645,
        0xedc967651c4938cc, 0x4e006ab71a48b85e, 0x193f621602de413c,
        0xb56458b71d56944f, 0xf2b639509a2fa5da, 0xb4a76f284c365450,
        0x4d3b65d2d2ae22f7, 0xbcc5f8303efca485, 0x8a044f312671aaea,
        0x688d69e89af0f57a, 0x229957dc1facede8, 0x2ed75c321073da13,
        0xf199e7ece5fcefef, 0x50c85b5c837a6c64, 0x71703c6e676bf698,
        0xc1b4eb52b1e5a518, 0x0f46a5e6c9cb68ca, 0xebb933688d69d7f7,
        0x5ab7404b8d1e3ef4, 0x261acc20c5a64a90, 0xb88788798adc718a,
        0x3e44e9b6bad5bc15, 0xf6bb456f086346bc, 0xd66e17e5734cbde1,
        0x392036dae96e389d, 0x4a62ceac9d4202de, 0x9d55f412f32e5f6e,
        0x0e1d841509d9ee9d, 0xc3130bdc638ed9e2, 0x0cd0e82af24964d9,
        0x3ec4c59463ba9b50, 0x055bc4d8685ab1bc, 0xb9e343c96a3a4253,
        0x8eba190d8688f7f9, 0xd31df36c792c629b, 0xddf82f659b127104,
        0x6f12dc8ba930fbb7, 0xa0aee6bb7e81a7f0, 0x8c6ba78747ae8777,
        0x86f00167eda1f9bc, 0x3a6f8b8f8a3790c9, 0x7845bb4a1c3bfbbb,
        0xc875ab077f66cf23, 0xa68b83d8d69b97ee, 0xb967199139f9a0a6,
        0x8a3a1a4d3de036b7, 0xdf3c5c0c017232a4, 0x8e60e63156990620,
        0xd31b4b03145f02fa,
    ];

    private static readonly ulong[] Table1 =
    [
        0xd1a16514dc206650, 0x4ddab180952e6a74, 0x7ed1d26a9f4a2d9b,
        0x2cc94adb288d1aec, 0x4339166e5035ca2e, 0xab9091be05d6f529,
        0xc75ffd54c19b9516, 0x207c8975c69bc35b, 0x08db87e31402eadc,
        0xfe4cd514c456d564, 0xd1420ff8db82c911, 0xade479a619a7636e,
        0x7217bae7640502f8, 0x17301bb4b2c9ff93, 0xe2f67d4293bea7c9,
        0x60c1b64503f1ae5f, 0xd3317230c6a89a4a, 0x54b6d40bb023d9ff,
        0x553f965dd89c9402, 0x322cf2b15d470119, 0xedf13527b6437e84,
        0x76745c0906d407c4, 0xc60da1497539ab11, 0x0ba7776f4792f8e5,
        0x2a7f551d8c5b4d31, 0x0cfead877ab063d5, 0x6e678a7838bb038b,
        0x875ed8d97acaebe7, 0x8aed1a12d4c3fbca, 0xe023e6ab9feeb8df,
        0xfdd21afbe2392ccf, 0x5fef7d88af28f38a, 0xe66d9569dc755d82,
        0x069488f524a68608, 0x4edb62a389057897, 0x970b0ec0227c306f,
        0xe2a50bdde312c3e8, 0x897a3917ad2d95e2, 0x4042ac5f9b83f96e,
        0x0325ab80db98eb7b, 0x842a6d834f0c225e, 0xfeedbeed9dae9a7d,
        0x8a12e02102a0b76c, 0xbfca933b7e85c4c8, 0x4d2d5efc211816d4,
        0x46b9a972083da108, 0xe0739db4c30eea4e, 0xbe9572ba55ba8a79,
        0x192340397c988b6f, 0xefc5bc5fa4408630, 0xfb5bc99628d59ad4,
        0xe53ec3ad106555d0, 0xacd2e5528d1c8edb, 0xded83bd80e3f6a80,
        0xd4f999f0069cd46d, 0xd4583576d6033deb, 0xdc6db7c69e83e859,
        0x59a1c8d2f3490df6, 0x4c8a746a098326f4, 0xfad28df33893006b,
        0x904406e32ab8dc66, 0x8ccb9c51392b512d, 0xeadb1fb4b22fddad,
        0x9240f9574c68d7f5, 0xc810baaa9aafe71b, 0x56fb3e2ca18ddd82,
        0x4131043a56f2ab16, 0x8992641f86c4e1c7, 0xfb37ec17d4baf904,
        0x63130ee438ae0f10, 0xf508d541c77c4c75, 0x68c415cfdfada5a7,
        0x07435b1908702546, 0x648800f682df4c01, 0x61ffa59e602118df,
        0xd787335b4289b86a, 0x9e62fb0ae8275291, 0x232e312192042bcb,
        0xa383b19bb4d06311, 0xdf5533274a5e899f, 0xfc3ecc60b3379191,
        0x014a552136e40013, 0x03cac0e8af12ef81, 0x3b682bf35abc651a,
        0x153a15d84143c296, 0x2b75664875bc5c44, 0xc1ff08df175bdba0,
        0xc28f94198b8c5bc5, 0x33764ca1509cee8a, 0x361cca0a9f8cb012,
        0x8ac1670c9498daa4, 0x2b63bccce71835d0, 0xc44743d726efe9f9,
        0x640e024d2e15ccc1, 0x29d4c518c0030776, 0xe139450316ac7e78,
        0xe395c05c9c34258d, 0x9d17e41d97809999, 0x88c6cbdd1a3c6ce9,
        0x962aa19bd5b0cb1e, 0x5e57e8667fde63e8, 0x6b9eafafa32707ed,
        0x1b20adeb05623a92, 0xf1a640886be62cf3, 0x05f655d4ebc1e835,
        0x177e24ea167a3da7, 0x0da18f53577cb417, 0x074828ada929b1ec,
        0x7d41698b0752fa52, 0xb71d01536265804d, 0x3f4f7448de07abf9,
        0x2dd1488ddc7c6e30, 0x2fe3573da7b77cc9, 0x55b9f117d610c05b,
        0x4db621a35cbf59ee, 0x96a2af56523d1d2f, 0xf4a744db21c8eedb,
        0xfaf92425521e6566, 0x5e1c09452e85986d, 0x9ee7e422fd0042d1,
        0x3e98973ebf9498d2, 0x84b60bd37abb6f17, 0x6e24b64e2acb1d81,
        0x486aa3cbbf85ac64, 0x71f2d26b9b0c9bab, 0x930f6a821e0ee6f8,
        0x0140107ee1eb554a, 0x55e01d909bb46204, 0x06125038ff7567a8,
        0x00e4eccdff895174, 0x39577406caec0504, 0x5a69e9aafe9a8e22,
        0xb30e393d5b9be940, 0x26cc2166e880cd7c, 0xe9b7fed59189881d,
        0x81a9a42005b1d5bc, 0xb2b82891717da7b2, 0x29cf375fe33643a3,
        0xd855452047893512, 0x2aefd7a703a749e6, 0xe135c6dbb52f9b43,
        0x22a8943e8e553598, 0x3ec40771a98728c6, 0x0299d18b770d2153,
        0x17d48b6bfe72a3d5, 0x24f07ecde7c3012d, 0x68323e439a196a35,
        0x7f5b0c6639f764e3, 0xa82d0805ac3b581d, 0x2ab139ec02b9f48e,
        0xa7d49bc197741231, 0x773e5f61bfac6c5f, 0xfa594af4be83d15e,
        0x6ecc4b7dbffa739f, 0x1865c8e2f94f345b, 0x9a3e90d88465170f,
        0x58c7b269955e9399, 0x4ff674d2780706a2, 0x455cbc9d3a26d266,
        0x6db8efbc17a17de7, 0xab1815bb95d92f95, 0x27f722912951f49e,
        0x5012c0a02442d9a5, 0x56583c2ad17a9d70, 0xd98debe4c52efe4e,
        0x99b7d5deddab7ee0, 0x85053d6e7721ee0e, 0x405af699372d70fc,
        0xcfac3959e13dcde3, 0xdb81bcf35a21461f, 0x011f1cce71738957,
        0xfcb2987cae0be3af, 0xf6935b228f0dc9dc, 0xa1e8f1cdac3c419a,
        0x3d41d54374c0a785, 0x696a68d1427c79e4, 0x77612ac115f0dabf,
        0xb17f39f98aca63ee, 0x1683a39ad034d6f3, 0x9e08ada4f55ac364,
        0x93bdc0f58a4ca761, 0x9510b346802b60c1, 0xdadf342305fd2a34,
        0x671800567cb99f7f, 0xb5c5363da88ab452, 0x0083bb39fb8f8024,
        0xb0b3dabd9f1841c0, 0x29747a3659b0aa27, 0x81906dfcfce396d6,
        0x06ee8cff0c633218, 0xe661c3160d389b8e, 0x577432f794816eeb,
        0x8bd7ffa707da0c0c, 0x2a3872e640f7cb2d, 0x815876c1a4de4351,
        0x6a8c240e8fec210b, 0xef2db3668fd73aec, 0x6d21b323856d8713,
        0xfd33d36416600d5f, 0xd513562868570a05, 0x9de4dd38c1ff025c,
        0xff835dc7599e827e, 0x1a17135a3c11df40, 0x1f4a2b5645503c81,
        0x4c0ff8ce6e28b27f, 0x2c0b30b9c350e9ee, 0xeb8eb60de4e6a93b,
        0x916c0f72057967f1, 0x15ae40496653de12, 0xac7a8a7023e8351e,
        0xa46dd5ec367648e4, 0xc486e30392e894bb, 0x2eb6490d594f48c1,
        0xc352ae1e0b627c8f, 0x97d9cd76b487b833, 0x77abb6c8f1659e98,
        0x17a8d48abb530f9a, 0x6c05900b7a4eca7a, 0xaa7a0e9a3dec62f2,
        0x99f6010404b9a3a2, 0x6db16d9cc11fa590, 0x88eb992abfca940a,
        0x6575749821bbcb5c, 0x6c3726b4adacf370, 0x52933b292d31756c,
        0x27c495baf3679571, 0x6ff0eb582e25528f, 0x3d9341714c0f84d2,
        0x8dcfdf60a27d5360, 0xada55b1f10a215b5, 0x4d93651bbcfc82b7,
        0x9494ec69a9020b07, 0x0de06b986239c3f7, 0xbb1f3424c66b5430,
        0x90f0b1362593f6a3, 0xefb0a868cbdde00d, 0x60e76f663824826c,
        0x78e635fb534f0f19, 0x291f832371aef5ec, 0x3c6c84ae20e63084,
        0x9bfcc6f845ae22d5, 0xe3e27c8d935b3747, 0x9a7efd84b0bf91de,
        0xee82fdbe4137cfe4, 0x9a5bdf951ed2bb7c, 0x542ca5a22dd37bcf,
        0x43d201d5c115c883, 0x88c3656920e8a0d4, 0xd5de586a7c7f9d38,
        0xbbdcccb1a9ffe942, 0xc2274eaf06595785, 0xcea374f00a2e840c,
        0x39cdeba6743f033a, 0x409adf6a28300f82, 0x4bc5012751032c2f,
        0x08c956ac24d7ba44,
    ];

    private static readonly IReadOnlyList<ulong[]> Tables =
    [
        Table0,
        Table1,
    ];

    public static GearTable Create(int rotations, int tableIndex)
    {
        if (tableIndex < 0 || tableIndex >= Tables.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(tableIndex), "Table index is out of range.");
        }

        var targetTable = Tables[tableIndex];

        var table = new ulong[targetTable.Length];

        if (rotations is < 0 or > 63)
        {
            throw new ArgumentException("Rotations must be in range [0; 63].", nameof(rotations));
        }

        for (var index = 0; index < table.Length; index++)
        {
            table[index] = BitOperations.RotateRight(targetTable[index], rotations);
        }

        return new GearTable(table);
    }
}
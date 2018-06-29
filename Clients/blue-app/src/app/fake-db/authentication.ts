export class AuthenticationFakeDb {
    public static users = [
        {
            'id'            : '5A61F008-0CE1-4B60-8C0C-C12C721E475D',
            'name'          : 'admin',
            'description'   : 'Admin',
            'active'      : true
        }
    ]
    
    public static roles = [
        {
            'id'            : '5A61F008-0CE1-4B60-8C0C-C12C721E475D',
            'code'          : 'admin',
            'name'          : 'admin',
            'description'   : 'Admin',
            'active'        : true
        },
        {
            'id'            : '7A749297-4EDF-4D16-A769-D3BADA83247E',
            'code'          : 'customer',
            'name'          : 'customer',
            'description'   : 'Customer',
            'active'        : true
        },
        {
            'id'            : '61F3DC6E-3863-40E5-BA2B-A6334B5590AC',
            'code'          : 'guest',
            'name'          : 'guest',
            'description'   : 'Guest',
            'active'        : true
        }
    ]
}
#import "KeyChainPlugin.h"
#import "UICKeyChainStore.h"
 
NSString *_keyForID = @"UserID";
NSString *_keyForUUID = @"UserUUID";
 
@implementation KeyChainPlugin
 
extern "C" {
    void setKeyChain(const char* _key, const char* _val);
    char* getKeyChain(const char* key);
    void deleteKeyChain(char* key);
    
    void setShareKeyChain(const char* _key, const char* _val, const char* _group);
    char* getShareKeyChain(const char* key, const char* group);
    void deleteShareKeyChain(char* key, const char* group);
    
    char* getKeyChainUser();
    void setKeyChainUser(const char* userId, const char* uuid);
    void deleteKeyChainUser();
}
 
void setKeyChain(const char* _key, const char* _val)
{
        NSString *key = [NSString stringWithCString: _key encoding:NSUTF8StringEncoding];
        NSString *val = [NSString stringWithCString: _val encoding:NSUTF8StringEncoding];
     
        [UICKeyChainStore setString:val forKey:key];
}

char* getKeyChain(const char* key)
{
        NSString *val = [UICKeyChainStore stringForKey:[NSString stringWithCString:key encoding:NSUTF8StringEncoding]];
     
        if (val == nil || [val isEqualToString:@""]) {
                val = @"";
        }
     
        return makeStringCopy([val UTF8String]);
}



void setShareKeyChain(const char* _key, const char* _val, const char* _group)
{
    
        NSString *key = [NSString stringWithCString: _key encoding:NSUTF8StringEncoding];
        NSString *val = [NSString stringWithCString: _val encoding:NSUTF8StringEncoding];
        NSString *group = [NSString stringWithCString: _group encoding:NSUTF8StringEncoding];
     
        [UICKeyChainStore setString:val forKey:key service:nil accessGroup:group];
}

char* getShareKeyChain(const char* key, const char* group)
{
    
      NSString *val = [UICKeyChainStore stringForKey:[NSString stringWithCString:key encoding:NSUTF8StringEncoding]
                                             service:nil accessGroup:[NSString stringWithCString:group encoding:NSUTF8StringEncoding]];
     
        if (val == nil || [val isEqualToString:@""]) {
                val = @"";
    }
     
        return makeStringCopy([val UTF8String]);
}

void deleteShareKeyChain(char* key, const char* group)
{
        [UICKeyChainStore removeItemForKey:[NSString stringWithCString:key encoding:NSUTF8StringEncoding]
                                   service:nil
                               accessGroup:[NSString stringWithCString:group encoding:NSUTF8StringEncoding]];
}

char* getKeyChainUser()
{
    NSString *userId = [UICKeyChainStore stringForKey:_keyForID];
    NSString *userUUID = [UICKeyChainStore stringForKey:_keyForUUID];
 
    if (userId == nil || [userId isEqualToString:@""]) {
        NSLog(@"No user information");
        userId = @"";
        userUUID = @"";
    }
 
    NSString* json = [NSString stringWithFormat:@"{\"userId\":\"%@\",\"uuid\":\"%@\"}",userId,userUUID];
 
    return makeStringCopy([json UTF8String]);
}
 
void setKeyChainUser(const char* userId, const char* uuid)
{
    NSString *nsUseId = [NSString stringWithCString: userId encoding:NSUTF8StringEncoding];
    NSString *nsUUID = [NSString stringWithCString: uuid encoding:NSUTF8StringEncoding];
 
    [UICKeyChainStore setString:nsUseId forKey:_keyForID];
    [UICKeyChainStore setString:nsUUID forKey:_keyForUUID];
}
 
void deleteKeyChain(char* key)
{
        [UICKeyChainStore removeItemForKey:[NSString stringWithCString:key encoding:NSUTF8StringEncoding]];
}

void deleteKeyChainUser()
{
    [UICKeyChainStore removeItemForKey:_keyForID];
    [UICKeyChainStore removeItemForKey:_keyForUUID];
}
 
char* makeStringCopy(const char* str)
{
    if (str == NULL) {
        return NULL;
    }
 
    char* res = (char*)malloc(strlen(str) + 1);
    strcpy(res, str);
    return res;
}
 
@end
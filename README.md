This assessment checks if Orleans could have derived Grain interfaces.

`ILocalAccount` and `IRemoteAccount` are two types of accounts that derive from `IAccountProxy`.

These two types should be successfully instantiated directly, casted to `IAccountProxy` and holded by other Grains.

``` csharp
var localAccount = (IAccountProxy) client.GetGrain<ILocalAccount>(Guid.NewGuid());
Debug.Assert(await localAccount.GetAccountType() == AccountType.Local);

var localAccountHolder = client.GetGrain<IAccountProxyHolder>(Guid.NewGuid());
await localAccountHolder.Initialize(AccountType.Local);
Debug.Assert(await localAccountHolder.GetAccountType() == AccountType.Local);
```

`ILocalAccount.GetAccountType()` is inherited from `IAccountProxy` and implemented in `LocalAccountGrain`.
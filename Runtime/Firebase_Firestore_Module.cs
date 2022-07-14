#region Access
using System;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Runtime.CompilerServices;
#endregion
namespace MMA.Firebase_Firestore
{
    [FirestoreData]
    public struct Test
    {
        [FirestoreProperty]
        public string _Test { get; set; }
    }
    public static class Key
    {
        // public const string _   = KeyData._;
        public static string Initialize = "Firebase_Firestore_Initialize";
        public static string Set = "Firebase_Firestore_Set";
        public static string Get = "Firebase_Firestore_Get";
    }
    public static class Import
    {
        //public const string _ = _;
    }
    public sealed partial class Firebase_Firestore_Module : Module
    {
        #region References
        //[Header("Applications")]
        //[SerializeField] public ApplicationBase interface_Firebase_Firestore;
        private FirebaseFirestore db = default;

        #endregion
        #region Reactions ( On___ )
        // Contenedor de toda las reacciones del Firebase_Firestore
        protected override void OnSubscription(bool condition)
        {
            //Initialize
            Middleware.Subscribe_Publish(condition, Key.Initialize, Initialize);

            //Set
            Middleware<(string path, object value)>.Subscribe_Task(condition, Key.Set, Set);
            Middleware<(string path, IDictionary<string, object> value)>.Subscribe_Task(condition, Key.Set, Set);
            //Middleware<(string path, ITuple value)>.Subscribe_Task(condition, Key.Set, Set);
            
            ////Get
            Middleware<string, object>.Subscribe_Task(condition, Key.Get, Get);
            Middleware<(string path, object defaultValue), object>.Subscribe_Task(condition, Key.Get, Get);

            ////Subscribe
            //Middleware<(string path, bool condition, Action<object> callback)>.Subscribe_Publish(condition, Key.Subscribe, Subscribe);
        }
        #endregion
        #region Methods
        // Contenedor de toda la logica del Firebase_Firestore
        private void Initialize()
        {
            db = FirebaseFirestore.DefaultInstance;
        }
        #endregion
        #region Request ( Coroutines )
        // Contenedor de toda la Esperas de corutinas del Firebase_Firestore
        #endregion
        #region Task ( async )
        // Contenedor de toda la Esperas asincronas del Firebase_Firestore
        private async Task<object> Get(string path)
        {
            //DocumentSnapshot snapshot = ;
            //(await db.Document(path).GetSnapshotAsync()).ToDictionary(;
            //DocumentSnapshot snap = await db.Document(path).GetSnapshotAsync();


            //TODO aquí hay un po
            Dictionary<string,object> data = (await db.Document(path).GetSnapshotAsync()).ConvertTo<Dictionary<string, object>>();
            foreach (var item in data)
            {
                Debug.Log(item.Key);
            }

            return await db.Document(path).GetSnapshotAsync().ContinueWith(a => a.Result.ToDictionary());

            //object response = snap.ConvertTo<object>();
            ////object data = snapshot.ConvertTo<object>();
            //Debug.Log(response);
            //Debug.Log(data);
            //return response;
        }

        private async Task<object> Get((string path, object defaultValue) data)
        {
            Debug.Log("def");

            return (await Get(data.path)) ?? data.defaultValue;
        }
        private async Task Set((string path, object value) data)
        {
            //TODO revisar los casos donde hay structs (y buscar mandarlos sin los attributes
            await db.Document(data.path).SetAsync(data.value);
        }
        private async Task Set((string path, IDictionary<string,object> value) data)
        {
            await db.Document(data.path).SetAsync(data.value);
        }

        //private async Task Set((string path, ITuple value) data)
        //{
        //    //data.value.GetType().GetInterfaces().Contains(typeof(ITuple))
        //    Dictionary<string, object> dic_value = new Dictionary<string, object>();
        //    for (int i = 0; i < data.value.Length; i++) dic_value.Add(i.ToString(), data.value[i]);
        //    await db.Document(data.path).SetAsync(dic_value);
        //}
        #endregion
    }
}

/* de BD
 * 
 * 
 * /// <summary>
    /// Sync the information
    /// </summary>
    public ListenerRegistration Sync(Action<T> callback, string _id = default)
    {
        string _idToUse = _id ?? id ?? default; // si no encuentra ningún ID entonces toca añadir
        return DocRef(_idToUse).Listen(snap =>{
            if (snap.Exists)
            {
                T data = snap.ConvertTo<T>();
                data.id = _idToUse;
                callback?.Invoke(data);
            }
        });
    }

    #region CollectionsCalling (static)
    /// <summary>
    /// Returns an array of Docs
    /// </summary>
    /// <param name="callBack"></param>
    public void GetAll(Action<T[]> callBack)
    {
        ColRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Assert.IsNull(task.Exception);
            T[] data = _ConvertCollection(task.Result);
            callBack.Invoke(data);
        });
    }



    /// <summary>
    /// Apply the transformation of the collection
    /// </summary>
    private static T[] _ConvertCollection(QuerySnapshot snap)
    {
        DocumentSnapshot[] Ddata = snap.ToArray();
        T[] data = new T[Ddata.Length];
        for (int i = 0; i < data.Length; i++){
            data[i] = Ddata[i].ConvertTo<T>();
            data[i].id = Ddata[i].Id;
        }
        return data;
    }


 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Accumulate objects sent via RPC methods
/// </summary>
public class RPCAccumulator<AccumulatedType>
{

    public RPCAccumulator()
    {

        accumulation_table_ = new Dictionary<int, IList<AccumulatedType>>();

    }

    /// <summary>
    /// Accumulate a new item inside the table
    /// </summary>
    /// <param name="unique_id">The unique id of the accumulator</param>
    /// <param name="item">The item to add to the specified accumulator</param>
    public void Accumulate(int unique_id, AccumulatedType item)
    {

        if (accumulation_table_.ContainsKey(unique_id))
        {

            //Ok existing entry, adds a new item to the previous list

            accumulation_table_[unique_id].Add(item);
            
        }
        else
        {

            //New entry, just create a new list with the item into it and adds it to the table

            var accumulated_items = new List<AccumulatedType>();
            
            accumulated_items.Add(item);

            accumulation_table_.Add(unique_id, accumulated_items);

        }

    }

    /// <summary>
    /// Fetch the list accumulated so far and evicts the accumulation list from the memory
    /// </summary>
    /// <param name="unique_id">The id of the accumulation list to fetch</param>
    /// <returns>Returns the list of the accumulated items</returns>
    public IList<AccumulatedType> Fetch(int unique_id)
    {

        var accumulation_list = accumulation_table_[unique_id];

        accumulation_table_.Remove(unique_id);

        return accumulation_list;

    }

    /// <summary>
    /// Accumulates all the objects
    /// </summary>
    private IDictionary<int, IList<AccumulatedType>> accumulation_table_;
    
}

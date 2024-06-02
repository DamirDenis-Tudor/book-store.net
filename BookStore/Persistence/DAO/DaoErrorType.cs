/**************************************************************************
 *                                                                        *
 *  Description: DaoErrorType                                             *
 *  Website:     https://github.com/DamirDenis-Tudor/PetShop-ProiectIP    *
 *  Copyright:   (c) 2024, Damir Denis-Tudor                              *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

namespace Persistence.DAO;

/// <summary>
/// Enumeration representing various types of errors that can occur in the DAO (Data Access Object) layer.
/// </summary>
public enum DaoErrorType
{
    /// <summary>
    /// Indicates that the requested item was not found.
    /// </summary>
    NotFound,

    /// <summary>
    /// Indicates that the item is already registered.
    /// </summary>
    AlreadyRegistered,

    /// <summary>
    /// Indicates that the list is empty.
    /// </summary>
    ListIsEmpty,

    /// <summary>
    /// Indicates a general database error.
    /// </summary>
    DatabaseError,
    
    Duplicate
}
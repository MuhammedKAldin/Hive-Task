using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTodoApp
{
    public static class SearchHelper
    {
        public static bool TodoMatchesSearch(Todo todo, string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }

            string lowerSearch = searchText.ToLower();
            string lowerTitle = todo.Title.ToLower();
            string lowerDesc = todo.Description.ToLower();

            return lowerTitle.Contains(lowerSearch) || lowerDesc.Contains(lowerSearch);
        }

        public static List<Todo> FilterTodosBySearch(TodoList todoList, string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return todoList.Todos.Take(todoList.Count).ToList();
            }

            return todoList.Todos
                .Take(todoList.Count)
                .Where(todo => TodoMatchesSearch(todo, searchText))
                .ToList();
        }
    }
}


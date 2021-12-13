package com.joonatanritalahti.trafficapp;

import android.content.Context;
import android.content.Intent;
import android.location.Location;
import android.os.Bundle;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.android.material.snackbar.Snackbar;

import android.util.JsonReader;
import android.util.Log;
import android.view.View;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.core.content.ContextCompat;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;
import androidx.navigation.ui.AppBarConfiguration;
import androidx.navigation.ui.NavigationUI;

import com.google.android.material.navigation.NavigationView;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import androidx.drawerlayout.widget.DrawerLayout;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;

import android.view.Menu;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.StringReader;
import java.util.Calendar;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;

public class Main3Activity extends AppCompatActivity {

    private AppBarConfiguration mAppBarConfiguration;

    LocationProvider locationProvider = new LocationProvider();
    Location curLoc;
    TextView tv;
    ImageView imageView;
    String text = "test";
    String name1 = "json.txt";
    String name2 = "stations.txt";
    Button refreshButton;
    TextView mainText;
    double valueTraffic = 0;
    TrafficCalculator trafficCalculator = new TrafficCalculator();
    TrafficStationObject targetObject;
    TrafficPlace placeObject;
    boolean clicked = false;
    ListView lv;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main3);
        Toolbar toolbar = findViewById(R.id.toolbar);
        lv = (ListView)findViewById(R.id.listView);
        setSupportActionBar(toolbar);
        FloatingActionButton fab = findViewById(R.id.fab);
        tv = (TextView)findViewById(R.id.traffic);
        tv.setText(getResources().getString(R.string.loading));
        imageView = (ImageView)findViewById(R.id.imageView);
        //refreshButton = (Button)findViewById(R.id.button);
        mainText = (TextView)findViewById(R.id.mainText);
        mainText.setText(getResources().getString(R.string.loading));

        RefreshListView();
        Refresh();

        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Snackbar.make(view, getResources().getString(R.string.loading), Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
                Refresh();
            }
        });
        DrawerLayout drawer = findViewById(R.id.drawer_layout);
        NavigationView navigationView = findViewById(R.id.nav_view);

        mAppBarConfiguration = new AppBarConfiguration.Builder(
                R.id.nav_home, R.id.nav_gallery, R.id.nav_slideshow,
                R.id.nav_tools, R.id.nav_share, R.id.nav_send)
                .setDrawerLayout(drawer)
                .build();
        NavController navController = Navigation.findNavController(this, R.id.nav_host_fragment);
        NavigationUI.setupActionBarWithNavController(this, navController, mAppBarConfiguration);
        NavigationUI.setupWithNavController(navigationView, navController);


    }
    //Refreshing the side panel list view for new sensors.
    public void RefreshListView(){
        String[] asd = null;
        if(placeObject!= null) {
            asd = new String[placeObject.getFeatures().size()];
            int countr = 0;
            for (Feature a : placeObject.getFeatures()
            ) {

                asd[countr] = a.getProperties().getNames().getFi();
                Log.d("LOG", "" + asd.length);
                countr++;
            }
            countr = 0;

        if(readFromFile(name2) != null && readFromFile(name2) != "" && lv != null) {
            placeObject = new Gson().fromJson(readFromFile(name2),TrafficPlace.class);
            lv.setAdapter(new ArrayAdapter<String>(
                            this,
                            android.R.layout.simple_list_item_1, asd
                    )
            );

            lv.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                @Override
                public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                    Log.d("LOG", "onItemClick(" + i + ")");
                    clicked = true;
                    Feature temp = placeObject.getFeatures().get(i);
                    trafficCalculator.closestPlace = temp;
                    trafficCalculator.closestStationId = temp.getProperties().getRoadStationId();
                    Refresh();
                }
            });
        }
        }
    }
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main3, menu);
        return true;
    }

    @Override
    public boolean onSupportNavigateUp() {
        NavController navController = Navigation.findNavController(this, R.id.nav_host_fragment);
        return NavigationUI.navigateUp(navController, mAppBarConfiguration)
                || super.onSupportNavigateUp();
    }
    //Refreshing the online data
    public void RefreshData(){
        RequestQueue queue = Volley.newRequestQueue(this);
        String url = "https://tie.digitraffic.fi/api/v1/data/tms-data";
        String url2 = "https://tie.digitraffic.fi/api/v3/metadata/tms-stations";

        StringRequest stringRequest = new StringRequest(Request.Method.GET, url,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        // Display the first 500 characters of the response string.
                        //tv.setText("Response is: "+ response.substring(0,500));
                        //text = response.substring(0,500);
                        writeToFile(response,name1);
                        //Log.d("LOG",""+response.length() + "-- " + response.substring(0,50));
                        //tv.setText("File text is: " + readFromFile());
                        //TrafficStationObject targetObject = gsonBuilder.create().fromJson(response,TrafficStationObject.class);
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                //tv.setText(error.getMessage());
                text = error.getMessage();
            }
        });
        StringRequest stringRequest2 = new StringRequest(Request.Method.GET, url2,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        // Display the first 500 characters of the response string.
                        //tv.setText("Response is: "+ response.substring(0,500));
                        //text = response.substring(0,500);
                        writeToFile(response,name2);
                        //Log.d("LOG",""+response.length() + "-- " + response.substring(0,50));
                        //tv.setText("File text is: " + readFromFile());
                        //TrafficStationObject targetObject = gsonBuilder.create().fromJson(response,TrafficStationObject.class);
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                //tv.setText(error.getMessage());
                text = error.getMessage();
            }
        });
        queue.add(stringRequest);
        queue.add(stringRequest2);

    }
    //Refresh screen
    public void Refresh(){
        curLoc = locationProvider.CheckLocation(this);
        tv.setText(getResources().getString(R.string.loading));
        RefreshData();
        if(!clicked)
        {
            RefreshListView();
        }
        mainText.setText(getResources().getString(R.string.loading));

        GsonBuilder gsonBuilder = new GsonBuilder();
        gsonBuilder.setLenient();
        Gson gson = gsonBuilder.create();

        targetObject = gson.fromJson(readFromFile(name1),TrafficStationObject.class);
        placeObject = gson.fromJson(readFromFile(name2),TrafficPlace.class);
        if(targetObject != null || placeObject != null) {
            valueTraffic = trafficCalculator.TrafficValue(targetObject, placeObject, curLoc,clicked);
            clicked = false;
            if (valueTraffic > 90) {
                mainText.setText(getResources().getString(R.string.traffic) + " " + getResources().getString(R.string.traffic1));
                imageView.setColorFilter(ContextCompat.getColor(this, R.color.blue));
            } else if (valueTraffic > 75) {
                mainText.setText(getResources().getString(R.string.traffic) + " " + getResources().getString(R.string.traffic2));
                imageView.setColorFilter(ContextCompat.getColor(this, R.color.yellow));
            } else if (valueTraffic > 25) {
                mainText.setText(getResources().getString(R.string.traffic) + " " + getResources().getString(R.string.traffic3));
                imageView.setColorFilter(ContextCompat.getColor(this, R.color.orange));
            } else if (valueTraffic > 10) {
                mainText.setText(getResources().getString(R.string.traffic) + " " + getResources().getString(R.string.traffic4));
                imageView.setColorFilter(ContextCompat.getColor(this, R.color.lightred));
            } else if (valueTraffic > 0) {
                mainText.setText(getResources().getString(R.string.traffic) + " " + getResources().getString(R.string.traffic5));
                imageView.setColorFilter(ContextCompat.getColor(this, R.color.darkred));
            } else {

            }
            //Log.d("LOG",""+trafficCalculator.TrafficValue(targetObject,placeObject,curLoc));
            if (trafficCalculator.permissionForLocation) {
                tv.setText(getResources().getString(R.string.text1) + " " + valueTraffic + getResources().getString(R.string.text2) + trafficCalculator.closestPlace.getProperties().getNames().getFi() + "\n" + getResources().getString(R.string.text3) + trafficCalculator.TimeLastCheck()+ "\n"+getResources().getString(R.string.text4)+" "+ Calendar.getInstance().getTime());
            } else if (trafficCalculator.permissionForLocation == false) {
                tv.setText(getResources().getString(R.string.textLocationPermissionNotGranted));
            }
        }else  {
            tv.setText("Not available");
        }
        //call for traffic calc to calculate
        //trafficCalculator.TrafficValue(targetObject,placeObject,curLoc);
    }

    // Request a string response from the provided URL.


    public void ChangeText(String text){
        tv.setText(text);
    }

//Save data to be used offline
    private void writeToFile(String data,String name) {
        try {
            Context context = this;

            OutputStreamWriter outputStreamWriter = new OutputStreamWriter(context.openFileOutput(name, Context.MODE_PRIVATE));
            Log.d("LOG","encoding " + outputStreamWriter.getEncoding());
            outputStreamWriter.write(data);
            outputStreamWriter.close();
        }
        catch (IOException e) {
            Log.d("LOG", "File write failed: " + e.toString());
        }
    }

    private String readFromFile(String name) {
        Context context = this;
        String ret = "";

        try {
            InputStream inputStream = context.openFileInput(name);

            if ( inputStream != null ) {
                InputStreamReader inputStreamReader = new InputStreamReader(inputStream);
                BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
                String receiveString = "";
                StringBuilder stringBuilder = new StringBuilder();

                while ( (receiveString = bufferedReader.readLine()) != null ) {
                    stringBuilder.append("\n").append(receiveString);
                }

                inputStream.close();
                ret = stringBuilder.toString();
            }
        }
        catch (FileNotFoundException e) {
            Log.e("login activity", "File not found: " + e.toString());
        } catch (IOException e) {
            Log.e("login activity", "Can not read file: " + e.toString());
        }

        return ret;
    }

}
